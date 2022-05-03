using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Enities;
using Misc.Interfaces;
using WebPortal.Areas.Admin.Models;
using WebPortal.Extensions;
using WebPortal.Models;

namespace WebPortal.Areas.Admin.Pages.Institutions
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.Context.SmarterlabsContext _context;
        private readonly IUserContext userContext;

        public EditModel(DataAccess.Context.SmarterlabsContext context, IUserContext userContext)
        {
            _context = context;
            this.userContext = userContext;
        }

        [BindProperty]
        public InstitutionViewModel Institution { get; set; }

        public async Task<IActionResult> OnGetAsync(string viewId)
        {
            if (viewId == null)
            {
                return NotFound();
            }
            
            int parsedId = 0;
            var slugResponse = Helpers.Helpers.GetSlugResponse(viewId);

            if (slugResponse.Status == SlugResponseStatus.Existing)
            {
                parsedId = slugResponse.Id;
            }

            var institutionToUpdate = await _context.Institutions
                .Include(i => i.Address)
                .Include(i => i.Type).FirstOrDefaultAsync(m => m.Id == parsedId);

            if (institutionToUpdate == null)
            {
                return NotFound();
            }
            Institution = institutionToUpdate.ToViewModel();
            ViewData["Types"] = new SelectList(_context.InstitutionTypes, "Id", "Name");
            ViewData["Countries"] = new SelectList(_context.Countries, "Name", "Name");
            ViewData["Statuses"] = new SelectList(_context.SystemStatuses, "Id", "Name");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
           
            if (!ModelState.IsValid)
            {
                ViewData["Types"] = new SelectList(_context.InstitutionTypes, "Id", "Name");
                ViewData["Countries"] = new SelectList(_context.Countries, "Name", "Name");
                ViewData["Statuses"] = new SelectList(_context.SystemStatuses, "Id", "Name");
                return Page();
            }            

            try
            {
                var institutionToUpdate = await _context.Institutions.Where(p => p.Id == Institution.Id.Value).Include(p=> p.Address).FirstOrDefaultAsync();
                if (!InstitutionExists(Institution.Id.Value))
                {
                    return NotFound();
                }
                institutionToUpdate.Name = Institution.Name;
                institutionToUpdate.TypeId = Institution.TypeId;
                if (Institution.SystemStatusId != null)
                {
                    institutionToUpdate.SystemStatusId = Institution.SystemStatusId.Value;
                }
                if (institutionToUpdate.Address == null)
                {
                    institutionToUpdate.Address = new DataAccess.Enities.Address()
                    {
                        Address1 = Institution.Address.Address1,
                        Address2 = Institution.Address.Address2,
                        Address3 = Institution.Address.Address3,
                        Country = Institution.Address.Country,
                        Created = DateTime.Now,
                        CreatedById = userContext.GetCurrentUser("").Id,
                    };
                }
                else
                {
                    institutionToUpdate.Address.Address1 = Institution.Address.Address1;
                    institutionToUpdate.Address.Address2 = Institution.Address.Address2;
                    institutionToUpdate.Address.Address3 = Institution.Address.Address3;
                    institutionToUpdate.Address.Country = Institution.Address.Country;  

                }


                _context.Institutions.Update(institutionToUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstitutionExists(Institution.Id.Value))
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

        private bool InstitutionExists(int id)
        {
            return _context.Institutions.Any(e => e.Id == id);
        }
    }
}
