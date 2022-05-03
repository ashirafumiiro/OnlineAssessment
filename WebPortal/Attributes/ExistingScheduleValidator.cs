using DataAccess.Context;
using Misc.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebPortal.Areas.Students.Models;

namespace WebPortal.Attributes
{
    public class ExistingScheduleValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (validationContext.ObjectInstance is ScheduleLabModel)
            {
                var model = (ScheduleLabModel)validationContext.ObjectInstance;

                if (model != null)
                {
                    var context = (SmarterlabsContext)validationContext.GetService(typeof(SmarterlabsContext));
                    var userContext = (IUserContext)validationContext.GetService(typeof(IUserContext));
                    var user = userContext.GetCurrentUser("");

                    var existingSchedules = context.Schedules.Where(p => p.SystemStatusId == (int)SM.Enumerations.SystemStatus.Active)
                        .Where(p => p.UserId == user.Id && p.LabId == model.LabId).ToList();
                    if (existingSchedules.Any(p => p.StatusId == (int)SM.Enumerations.ScheduleStatus.Pending))
                        return new ValidationResult("Active Shedule already exists");

                    if (existingSchedules.Any(p => p.StatusId == (int) SM.Enumerations.ScheduleStatus.Completed))
                        return new ValidationResult("This lab has been completed");

                    if (existingSchedules.Any(p => p.StatusId == (int)SM.Enumerations.ScheduleStatus.Missed))
                        return new ValidationResult("This lab has been missed. Contact Administrator");

                    return ValidationResult.Success;
                }
                
            }

            return new ValidationResult("Failed to validate schedule");
        }
    }
}
