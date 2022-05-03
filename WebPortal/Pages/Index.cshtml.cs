using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using DataAccess.Context;
using DataAccess.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPortal.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SmarterlabsContext context;

        public List<AspNetUser> Users { get; set; }

        public IndexModel(ILogger<IndexModel> logger, SmarterlabsContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public void OnGet()
        {
            this.Users = context.AspNetUsers.ToList();
        }
    }
}
