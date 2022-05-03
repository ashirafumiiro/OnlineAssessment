using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess.Context;
using DataAccess.Enities;
using Misc.Interfaces;
using WebPortal.Areas.Admin.Models;

namespace WebPortal.Areas.Admin.Pages.Labs
{
    public class CreateModel : PageModel
    {
        private readonly DataAccess.Context.SmarterlabsContext _context;
        private readonly IUserContext userContext;

        public CreateModel(DataAccess.Context.SmarterlabsContext context, IUserContext userContext)
        {
            _context = context;
            this.userContext = userContext;
        }

        public IActionResult OnGet()
        {
            ViewData["Institutions"] = new SelectList(_context.Institutions, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public LabViewModel Lab { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var user = userContext.GetCurrentUser("");
                var labToAdd = new Lab()
                {
                    Name = Lab.Name,
                    Token = Lab.Token,
                    TokenExpiration = Lab.TokenExpiration,
                    InstitutionId = Lab.InstitutionId,
                    StartDate = Lab.StartDate,
                    StopDate = Lab.StopDate,
                    MaximumTime = Lab.MaximumTime,
                    Created = DateTime.UtcNow,
                    CreatedById = user.Id,
                    SystemStatusId = 1,
                    Guid = Guid.NewGuid().ToString()
                };

                _context.Labs.Add(labToAdd);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
