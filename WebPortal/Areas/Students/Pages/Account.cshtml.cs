using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DataAccess.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebPortal.Models.ViewModels;

namespace WebPortal.Areas.Students.Pages
{
    public class AccountModel : PageModel
    {
        private readonly SmarterlabsContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger logger;

        [BindProperty]
        public UpdateAccountViewModel UserModel { get; set; }

        public AccountModel(SmarterlabsContext context, UserManager<IdentityUser> userManager, ILogger<AccountModel> logger)
        {
            this.context = context;
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task OnGet()
        {
            var identityUser = await userManager.GetUserAsync(HttpContext.User);

            var user = context.AspNetUsers.Where(p => p.Id == identityUser.Id).FirstOrDefault();

            UserModel = new UpdateAccountViewModel();

            if (user != null)
            {
                UserModel.Email = user.Email;
                UserModel.Id = user.Id;
                UserModel.FirstName = user.FirstName;
                UserModel.LastName = user.LastName;
                UserModel.PhoneNumber = user.PhoneNumber;
                UserModel.InstitutionId = user.InstitutionId ?? 1;
            }
            ViewData["InstitutionId"] = new SelectList(context.Institutions, "Id", "Name");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userToUpdate = context.AspNetUsers.FirstOrDefault(p => p.Id == UserModel.Id);
            if (userToUpdate == null)
            {
                return NotFound();
            }

            userToUpdate.FirstName = UserModel.FirstName;
            userToUpdate.LastName = UserModel.LastName;
            userToUpdate.PhoneNumber = UserModel.PhoneNumber;
            userToUpdate.InstitutionId = UserModel.InstitutionId;

            context.Attach(userToUpdate).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                logger.LogError(ex, ex.Message);

                if (!AspNetUserExists(userToUpdate.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            ViewData["InstitutionId"] = new SelectList(context.Institutions, "Id", "Name");
            return Page();

        }

        private bool AspNetUserExists(string id)
        {
            return context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
