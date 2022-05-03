using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Enities;
using WebPortal.Areas.Admin.Models;

namespace WebPortal.Areas.Admin.Pages.Roles
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.Context.SmarterlabsContext _context;
        private readonly RoleManager<IdentityRole> roleManager;

        public EditModel(DataAccess.Context.SmarterlabsContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            this.roleManager = roleManager;
        }

        [BindProperty]
        public RoleViewModel Role { get; set; }

        public async Task<IActionResult> OnGetAsync(string viewId)
        {
            if (viewId == null)
            {
                return NotFound();
            }

            var role = await roleManager.FindByIdAsync(viewId);
            if (role == null)
            {
                return NotFound();
            }


            Role = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };

            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
                
            try
            {
                var role = await roleManager.FindByIdAsync(Role.Id);
                if (role == null)
                {
                    return NotFound();
                }
                role.Name = Role.Name;

                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToPage("./Index");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return Page();
        }

    }
}
