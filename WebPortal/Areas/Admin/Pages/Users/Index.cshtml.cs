using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Enities;

namespace WebPortal.Areas.Admin.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly DataAccess.Context.SmarterlabsContext _context;

        public IndexModel(DataAccess.Context.SmarterlabsContext context)
        {
            _context = context;
        }

        public IList<AspNetUser> AspNetUser { get;set; }

        public async Task OnGetAsync()
        {
            AspNetUser = await _context.AspNetUsers
                .Include(a => a.Institution)
                .Include(a => a.SystemStatus)
                .Include(a => a.Type).ToListAsync();
        }
    }
}
