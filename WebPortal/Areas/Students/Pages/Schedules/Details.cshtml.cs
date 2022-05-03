using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Extensions;
using System.Linq;
using System.Threading.Tasks;
using WebPortal.Areas.Students.Models;
using WebPortal.Models;

namespace WebPortal.Areas.Students.Pages.Schedules
{
    public class DetailsModel : PageModel
    {
        private readonly SmarterlabsContext context;
        public ScheduleViewModel Schedule { get; set; }

        public DetailsModel(SmarterlabsContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> OnGet(string viewId)
        {
            var slugResponse = Helpers.Helpers.GetSlugResponse(viewId);

            if (string.IsNullOrEmpty(viewId) || slugResponse.Status != SlugResponseStatus.Existing)
            {
                return NotFound();
            }

            var scheduleToView = await context.Schedules.GetActive().Where(m => m.Id == slugResponse.Id).Include(p => p.Lab).FirstOrDefaultAsync();
            if (scheduleToView != null)
            {
                Schedule = new ScheduleViewModel()
                {
                    Id = scheduleToView.Slug,
                    LabName = scheduleToView.Lab.Name,
                    StartTime = scheduleToView.StartTime,
                    EndTime = scheduleToView.EndTime,
                    Status = (SM.Enumerations.ScheduleStatus)scheduleToView.StatusId,
                };
            }
            else
            {
                return NotFound();
            }

            return Page();
        }



    }
}
