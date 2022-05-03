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
using WebPortal.Models;

namespace WebPortal.Areas.Admin.Pages.Schedules
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.Context.SmarterlabsContext _context;

        public EditModel(DataAccess.Context.SmarterlabsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Schedule Schedule { get; set; }

        public async Task<IActionResult> OnGetAsync(string viewId)
        {
            if (string.IsNullOrEmpty(viewId))
            {
                return NotFound();
            }
            int parsedId = 0;

            var slugResponse = Helpers.Helpers.GetSlugResponse(viewId);

            if (slugResponse.Status == SlugResponseStatus.Existing)
            {
                parsedId = slugResponse.Id;
            }



            Schedule = await _context.Schedules
                .Include(s => s.Lab)
                .Include(s => s.Status)
                .Include(s => s.User).FirstOrDefaultAsync(m => m.Id == parsedId);

            if (Schedule == null)
            {
                return NotFound();
            }
           ViewData["LabId"] = new SelectList(_context.Labs, "Id", "Name");
           ViewData["StatusId"] = new SelectList(_context.ScheduleStatuses, "Id", "Name");
           ViewData["SystemStatuses"] = new SelectList(_context.SystemStatuses, "Id", "Name");
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

            _context.Attach(Schedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(Schedule.Id))
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

        private bool ScheduleExists(long id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }
    }
}
