using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DataAccess.Context;
using DataAccess.Enities;
using Misc.Interfaces;
using System;
using System.Linq;
using System.Security.Claims;

namespace WebPortal.Data
{
    public class HttpUserContext : IUserContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly SmarterlabsContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger<HttpUserContext> logger;
        AspNetUser user;
        public HttpUserContext(IHttpContextAccessor httpContextAccessor, SmarterlabsContext context, UserManager<IdentityUser> userManager,
            ILogger<HttpUserContext> logger)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
            this.userManager = userManager;
            this.logger = logger;
        }

        public AspNetUser GetCurrentUser(string includeProperties)
        {
            try
            {
                if (httpContextAccessor.HttpContext.Request != null && httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    if (user == null)
                    {
                        var contextUser = httpContextAccessor.HttpContext.User;
                        if (contextUser != null)
                        {
                            var userName = contextUser.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value;
                            var identityUser = userManager.FindByNameAsync(userName).Result;

                            var userQuery = context.AspNetUsers.Where(x => x.Email == identityUser.Email);

                            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                userQuery = userQuery.Include(includeProperty.Trim());
                            }

                            user = userQuery.FirstOrDefault();
                        }
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
            }
           
            return user;
        }

        public bool IsInRole(string role)
        {
            var returnvalue = false;

            if (this.user != null)
            {
                return this.user.Roles.Any(p => p.Role.Name == role);
            }

            return returnvalue;
        }
    }
}
