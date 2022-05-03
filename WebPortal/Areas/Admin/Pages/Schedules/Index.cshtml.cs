using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Enities;

namespace WebPortal.Areas.Admin.Pages.Schedules
{
    public class IndexModel : PageModel
    {
        private readonly DataAccess.Context.SmarterlabsContext _context;

        public IndexModel(DataAccess.Context.SmarterlabsContext context)
        {
            _context = context;
        }

        public IList<Schedule> Schedule { get;set; }

        public async Task OnGetAsync()
        {
            Schedule = await _context.Schedules
                .Include(s => s.Lab)
                .Include(s => s.Status)
                .Include(s => s.User).ToListAsync();
        }
    }
}
