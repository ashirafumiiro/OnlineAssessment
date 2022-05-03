using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using DataAccess.Context;
using DataAccess.Enities;
using DataAccess.Messaging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebPortal.Configuration;
using WebPortal.Models;

namespace WebPortal.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SmarterlabsContext context;


        public AuthController(IOptions<JwtBearerTokenSettings> jwtTokenOptions, UserManager<IdentityUser> userManager, SmarterlabsContext context)
        {
            this.context = context;
            this.userManager = userManager;
            this.jwtBearerTokenSettings = jwtTokenOptions.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Token([FromBody] LoginCredentials credentials)
        {
            var response = new ServiceMessage<LoginResponse>();

            if (!ModelState.IsValid)
            {
                string errors = JsonConvert.SerializeObject(ModelState.Values
                    .SelectMany(state => state.Errors)
                    .Select(error => error.ErrorMessage));

                response.ErrorOrWarningCode = "500";
                response.ErrorOrWarningNarrative = errors;
            }
            else
            {
                if (credentials == null)
                {
                    return this.BadRequest();
                }
                else
                {
                    var user = context.AspNetUsers.Where(p => p.Email.ToLower() == credentials.username.ToLower()).FirstOrDefault();
                    if (user == null)
                    {
                        response.ErrorOrWarningNarrative = "Invalid Username or password";
                        response.ErrorOrWarningCode = "401";
                        return this.Ok(response);
                    }
                        
                    var identityUser = await userManager.FindByEmailAsync(user.Email);

                    var validateCredentials = userManager.CheckPasswordAsync(identityUser, credentials.password);

                    if (validateCredentials.Result)
                    {
                        var applicationUser = context.AspNetUsers.Where(p => p.NormalizedEmail == user.UserName.ToUpper()).FirstOrDefault();

                        if (applicationUser != null)
                        {
                            var token = this.GenerateToken(identityUser, applicationUser);
                            response.Data = new LoginResponse() { Token = token, Success = true };    
                        }
                        else
                        {
                            response.ErrorOrWarningCode = "401";
                            response.ErrorOrWarningNarrative = "Invalid username or password";
                        }
                    }
                    else
                    {
                        response.ErrorOrWarningCode = "401";
                        response.ErrorOrWarningNarrative = "Invalid username or password";
                    }
                }
            }

            return this.Ok(response);
        }


        private string GenerateToken(IdentityUser identityUser, AspNetUser applicationUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);
            string normalizedEmail = identityUser.NormalizedEmail;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, identityUser.UserName.ToString()),
                    new Claim(ClaimTypes.Email, identityUser.Email),
                    new Claim("userName", identityUser.UserName),
                        new Claim("firstName", applicationUser.FirstName),
                        new Claim("lastName", applicationUser.LastName),
                        new Claim("email", applicationUser.Email),
                        new Claim("institutionId", applicationUser.InstitutionId != null ? applicationUser.InstitutionId.ToString() : "0"),
                        new Claim("phoneNumber", applicationUser.PhoneNumber),
                        new Claim("userId", applicationUser.Id),
                        new Claim("twoFactorEnabled", applicationUser.TwoFactorEnabled.ToString().ToLower())
                }),

                Expires = DateTime.UtcNow.AddDays(jwtBearerTokenSettings.ExpiryTimeInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.Audience,
                Issuer = jwtBearerTokenSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
