using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DataAccess.Context;
using DataAccess.Enities;
using Misc.Interfaces;
using WebPortal.Areas.Admin.Models;
using WebPortal.Extensions;

namespace WebPortal.Areas.Admin.Pages.Institutions
{
    public class CreateModel : PageModel
    {
        private readonly DataAccess.Context.SmarterlabsContext _context;
        private readonly ILogger<CreateModel> logger;
        private readonly IUserContext userContext;

        public CreateModel(DataAccess.Context.SmarterlabsContext context, ILogger<CreateModel> logger, IUserContext userContext)
        {
            _context = context;
            this.logger = logger;
            this.userContext = userContext;
        }

        public IActionResult OnGet()
        {
            ViewData["Types"] = new SelectList(_context.InstitutionTypes, "Id", "Name");
            ViewData["Countries"] = new SelectList(_context.Countries, "Name", "Name");
            return Page();
        }

        [BindProperty]
        public InstitutionViewModel Institution { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["Types"] = new SelectList(_context.InstitutionTypes, "Id", "Name");
                ViewData["Countries"] = new SelectList(_context.Countries, "Name", "Name");
                return Page();
            }
            try
            {
                var institutionToAdd = this.Institution.ToEntity();
                var user = this.userContext.GetCurrentUser("");
                institutionToAdd.CreatedById = user.Id;
                institutionToAdd.Address.CreatedById = user.Id;
                institutionToAdd.Created = DateTime.Now;
                institutionToAdd.Address.Created = DateTime.Now;
                institutionToAdd.SystemStatusId = 1;

                logger.LogInformation(JsonConvert.SerializeObject(institutionToAdd));
                _context.Institutions.Add(institutionToAdd);
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
