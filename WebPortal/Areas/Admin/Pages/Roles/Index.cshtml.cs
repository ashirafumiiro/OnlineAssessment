using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Enities;

namespace WebPortal.Areas.Admin.Pages.Roles
{
    public class IndexModel : PageModel
    {
        private readonly DataAccess.Context.SmarterlabsContext _context;

        public IndexModel(DataAccess.Context.SmarterlabsContext context)
        {
            _context = context;
        }


        //public async Task OnGetAsync()
        //{
            
        //}
    }
}
