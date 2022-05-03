using DataAccess.Context;
using DataAccess.Extensions;
using Misc.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebPortal.Areas.Admin.Models;
using WebPortal.Models;

namespace WebPortal.Attributes
{
    public class CanceActiveScheduleValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (validationContext.ObjectInstance is CancelScheduleModel)
            {
                var model = (CancelScheduleModel)validationContext.ObjectInstance;

                if (model != null)
                {
                    var context = (SmarterlabsContext)validationContext.GetService(typeof(SmarterlabsContext));
                    var userContext = (IUserContext)validationContext.GetService(typeof(IUserContext));
                    var user = userContext.GetCurrentUser("");

                    var slugResponse = Helpers.Helpers.GetSlugResponse(model.Id);

                    if (string.IsNullOrEmpty(model.Id) || slugResponse.Status != SlugResponseStatus.Existing)
                    {
                        return new ValidationResult("Validation not found");
                    }

                    var schedule = context.Schedules.GetActive()
                        .Where(p => p.Id == slugResponse.Id && p.StatusId == (int)SM.Enumerations.ScheduleStatus.Pending).FirstOrDefault();
                    var timeNow = System.DateTime.Now.ToUniversalTime();
                    if (schedule != null && schedule.StartTime.IsGreaterThan(timeNow))
                    {
                        return ValidationResult.Success;
                    }   
                    else
                        return new ValidationResult("Schedule cannot be Canceled");
                }

            }

            return new ValidationResult("Failed to validate schedule data");
        }
    }
}
