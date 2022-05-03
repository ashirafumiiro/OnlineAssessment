using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess.Context;
using DataAccess.Enities;
using WebPortal.Areas.Admin.Models;

namespace WebPortal.Areas.Admin.Pages.Roles
{
    public class CreateModel : PageModel
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public CreateModel(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public RoleViewModel Role { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            IdentityRole identityRole = new IdentityRole
            {
                Name = Role.Name
            };
            IdentityResult result = await roleManager.CreateAsync(identityRole);

            if (result.Succeeded)
            {
                return RedirectToPage("./Index");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return Page();
        }
    }
}
