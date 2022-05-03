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

namespace WebPortal.Areas.Admin.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly SmarterlabsContext _context;
        private readonly RoleManager<IdentityRole> roleManager;

        public EditModel(SmarterlabsContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            this.roleManager = roleManager;
        }

        [BindProperty]
        public UserViewModel UserModel{ get; set; }

        public async Task<IActionResult> OnGetAsync(string viewId)
        {
            if (viewId == null)
            {
                return NotFound();
            }

            var userToUpdate = await _context.AspNetUsers
                .Include(a => a.Institution)
                .Include(a => a.SystemStatus)
                .Include(a => a.Type).FirstOrDefaultAsync(m => m.Id == viewId);

            if (userToUpdate == null)
            {
                return NotFound();
            }

            UserModel = new UserViewModel()
            {
                Id = userToUpdate.Id,
                FirstName = userToUpdate.FirstName,
                LastName = userToUpdate.LastName,
                PhoneNumber = userToUpdate.PhoneNumber,
                InstitutionId = userToUpdate.InstitutionId != null ? userToUpdate.InstitutionId.Value : 0,
                SystemStatusId = userToUpdate.SystemStatusId,
                TypeId = userToUpdate.TypeId != null ? userToUpdate.TypeId.Value : 0,
                Email = userToUpdate.Email
            };

            ViewData["InstitutionId"] = new SelectList(_context.Institutions, "Id", "Name");
            ViewData["SystemStatusId"] = new SelectList(_context.SystemStatuses, "Id", "Name");
            ViewData["TypeId"] = new SelectList(_context.UserTypes, "Id", "Name");
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

            var userToUpdate = _context.AspNetUsers.FirstOrDefault(p => p.Id == UserModel.Id);
            if (userToUpdate == null)
            {
                return NotFound();
            }

            userToUpdate.FirstName = UserModel.FirstName;
            userToUpdate.LastName = UserModel.LastName;
            userToUpdate.PhoneNumber = UserModel.PhoneNumber;
            userToUpdate.InstitutionId = UserModel.InstitutionId;
            userToUpdate.SystemStatusId = UserModel.SystemStatusId;
            userToUpdate.TypeId = UserModel.TypeId;
            

            _context.Attach(userToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserExists(userToUpdate.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AspNetUserExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
