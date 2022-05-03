using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Enities;
using WebPortal.Areas.Admin.Models;
using WebPortal.Helpers;
using WebPortal.Models;

namespace WebPortal.Areas.Admin.Pages.Labs
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.Context.SmarterlabsContext _context;

        public EditModel(DataAccess.Context.SmarterlabsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LabViewModel Lab { get; set; }

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

            var labToEdit = await _context.Labs.FirstOrDefaultAsync(m => m.Id == parsedId);

            if (labToEdit == null)
            {
                return NotFound();
            }

            Lab = new LabViewModel()
            {
                Id = labToEdit.Id,
                InstitutionId = labToEdit.InstitutionId,
                MaximumTime = labToEdit.MaximumTime,
                Name = labToEdit.Name,
                StartDate = labToEdit.StartDate,
                StopDate = labToEdit.StopDate,
                Token = labToEdit.Token,
                TokenExpiration = labToEdit.TokenExpiration,
                Description = labToEdit.Description
            };

            ViewData["Institutions"] = new SelectList(_context.Institutions, "Id", "Name");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["Institutions"] = new SelectList(_context.Institutions, "Id", "Name");
                return Page();
            }

            try
            {
                var labToEdit = await _context.Labs
                    .FirstOrDefaultAsync(m => m.Id == Lab.Id);
                if (labToEdit == null)
                {
                    return NotFound();
                }

                labToEdit.Name = Lab.Name;
                labToEdit.StartDate = Lab.StartDate;
                labToEdit.StopDate = Lab.StopDate;
                labToEdit.Token = Lab.Token;
                labToEdit.MaximumTime = Lab.MaximumTime;
                labToEdit.TokenExpiration = Lab.TokenExpiration;
                labToEdit.InstitutionId = Lab.InstitutionId;
                labToEdit.Description = Lab.Description;


                _context.Labs.Update(labToEdit);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LabExists(Lab.Id.Value))
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

        private bool LabExists(int id)
        {
            return _context.Labs.Any(e => e.Id == id);
        }
    }
}
