using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using DataAccess.Context;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebPortal.Pages
{
    public class AccountModel : PageModel
    {
        private readonly SmarterlabsContext context;
        private readonly ILogger logger;
        private readonly UserManager<IdentityUser> userManager;

        public AccountModel(SmarterlabsContext context, ILogger<AccountModel> logger, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.logger = logger;
            this.userManager = userManager;
        }

        [BindProperty]
        public UserUpdateModel UserModel { get; set; }
        public IActionResult OnGet()
        {
            var identityUser = userManager.GetUserAsync(User).Result;
            if (identityUser != null)
            {
                var user = context.AspNetUsers.Where(p => p.Id == identityUser.Id).FirstOrDefault();
                if (user != null)
                {
                    UserModel = new UserUpdateModel()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        InstitutionId = user.InstitutionId ?? 0, 
                    };
                }
            }
            return Page();
        }

        public class UserUpdateModel
        {
            public string Id { get; set; }
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Institution Type")]
            public int InstitutionId { get; set; }

            [Required]
            [Display(Name = "Email/UserName")]
            public string Email { get; set; }
        }
    }
}
