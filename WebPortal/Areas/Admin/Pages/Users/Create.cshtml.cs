using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess.Context;
using DataAccess.Enities;

namespace WebPortal.Areas.Admin.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly DataAccess.Context.SmarterlabsContext _context;

        public CreateModel(DataAccess.Context.SmarterlabsContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["InstitutionId"] = new SelectList(_context.Institutions, "Id", "Name");
        ViewData["SystemStatusId"] = new SelectList(_context.SystemStatuses, "Id", "Name");
        ViewData["TypeId"] = new SelectList(_context.UserTypes, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public AspNetUser AspNetUser { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AspNetUsers.Add(AspNetUser);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
