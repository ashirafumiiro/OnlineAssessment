using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Enities;

namespace WebPortal.Areas.Admin.Pages.Institutions
{
    public class IndexModel : PageModel
    {
        private readonly DataAccess.Context.SmarterlabsContext _context;

        public IndexModel(DataAccess.Context.SmarterlabsContext context)
        {
            _context = context;
        }

        public IList<Institution> Institution { get;set; }

        public async Task OnGetAsync()
        {
            Institution = await _context.Institutions
                .Include(i => i.Address)
                .Include(i => i.Type).ToListAsync();
        }
    }
}
