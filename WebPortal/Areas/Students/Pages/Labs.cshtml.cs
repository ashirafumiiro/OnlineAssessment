using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DataAccess.Context;
using DataAccess.Enities;
using DataAccess.Extensions;
using Misc.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace WebPortal.Areas.Students.Pages
{
    public class LabsModel : PageModel
    {
        public List<Lab> CompletedLabs { get; set; }
        public List<Lab> MissedLabs { get; set; }
        public List<Lab> AvailableLabs { get; set; }
        public List<Schedule> Schedules { get; set; }

        private readonly SmarterlabsContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger logger;
        private readonly IUserContext userContext;

        public LabsModel(SmarterlabsContext context, UserManager<IdentityUser> userManager, ILogger<LabsModel> logger, IUserContext userContext)
        {
            this.context = context;
            this.userManager = userManager;
            this.logger = logger;
            this.userContext = userContext;
        }

        public void OnGet()
        {
            var user = userContext.GetCurrentUser("");
            CompletedLabs = new List<Lab>();
            Schedules = new List<Schedule>();
            MissedLabs = new List<Lab>();
            AvailableLabs = new List<Lab>();

            AvailableLabs = context.Labs.GetActive().
                Where(p => p.InstitutionId == user.InstitutionId  || p.InstitutionId == (int)SM.Enumerations.SmarterLabs.Id).ToList();

            
            var schedules = context.Schedules.GetActive().Where(p => p.UserId == user.Id).Include(p => p.Lab).ToList();
            Schedules = schedules;

            var completed = schedules.Where(p => p.UserId == user.Id && p.StatusId == (int)SM.Enumerations.ScheduleStatus.Completed).ToList();
            if (completed.Count > 0)
            {
                CompletedLabs = completed.Select(p => p.Lab).ToList();
            }

            var missed = schedules.Where(p => p.UserId == user.Id && p.StatusId == (int)SM.Enumerations.ScheduleStatus.Missed).ToList();
            if (missed.Count > 0)
            {
                MissedLabs = missed.Select(p => p.Lab).ToList();
            }

        }
    }
}
