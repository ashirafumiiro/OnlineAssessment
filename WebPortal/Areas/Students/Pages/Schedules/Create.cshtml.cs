using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DataAccess.Context;
using Misc.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebPortal.Areas.Admin.Models;
using WebPortal.Models;

namespace WebPortal.Areas.Students.Pages.Schedules
{
    public class CreateModel : PageModel
    {
        private readonly SmarterlabsContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger logger;
        private readonly IUserContext userContext;

        public CreateSheduleModel ScheduleModel { get; set; }

        public CreateModel(SmarterlabsContext context, UserManager<IdentityUser> userManager, ILogger<LabsModel> logger, IUserContext userContext)
        {
            this.context = context;
            this.userManager = userManager;
            this.logger = logger;
            this.userContext = userContext;
        }



        public async Task<IActionResult> OnGet(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int parsedId = 0;

            var slugResponse = Helpers.Helpers.GetSlugResponse(id);

            if (slugResponse.Status == SlugResponseStatus.Existing)
            {
                parsedId = slugResponse.Id;
            }

            var labToSchedule = await context.Labs.FirstOrDefaultAsync(m => m.Id == parsedId);

            if (labToSchedule == null)
            {
                return NotFound();
            }

            ScheduleModel = new CreateSheduleModel()
            {
                LabId = labToSchedule.Id,
                TimeInterval = labToSchedule.MaximumTime, //maximum minutes
                MaximumDate = labToSchedule.StopDate.Value,
                LabName = labToSchedule.Name
            };
             
            return Page();
        }
    }

    public class OwnerRes
    {
        public string text { set; get; }
        public int id { set; get; }
        public string color { set; get; }
    }
}
