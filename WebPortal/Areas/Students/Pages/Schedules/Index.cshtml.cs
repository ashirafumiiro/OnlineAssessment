using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Enities;
using DataAccess.Extensions;
using Misc.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPortal.Areas.Students.Pages.Schedules
{
    public class IndexModel : PageModel
    {
        private readonly SmarterlabsContext context;
        private readonly IUserContext userContext;

        public List<Schedule> Schedules { get; set; }

        public IndexModel(SmarterlabsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }
        public void OnGet()
        {
            var user = userContext.GetCurrentUser("");
            var schedules = context.Schedules.GetActive().Where(p => p.UserId == user.Id && p.StatusId == (int)SM.Enumerations.ScheduleStatus.Pending).Include(p => p.Lab).ToList();
            Schedules = schedules;

        }
    }
}
