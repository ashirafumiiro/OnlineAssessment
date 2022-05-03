using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using DataAccess.Context;
using System.Linq;
using System.Threading.Tasks;

namespace WebPortal.Areas.Students.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SmarterlabsContext context;
        private readonly ILogger<IndexModel> logger;
        private readonly UserManager<IdentityUser> userManager;

        public IndexModel(SmarterlabsContext context, ILogger<IndexModel> logger, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.logger = logger;
            this.userManager = userManager;
        }
        public string Name { get; set; }
        public async Task OnGetAsync()
        {
            var identityUser = await userManager.GetUserAsync(User);
            if (identityUser != null)
            {
                var user = context.AspNetUsers.Where(p => p.Id == identityUser.Id).FirstOrDefault();
                Name = $"{user.FirstName} {user.LastName}";
            }
        }
    }
}
