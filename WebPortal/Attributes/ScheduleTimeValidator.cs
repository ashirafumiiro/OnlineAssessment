using DataAccess.Context;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebPortal.Areas.Students.Models;

namespace WebPortal.Attributes
{
    public class ScheduleTimeValidator: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (validationContext.ObjectInstance is ScheduleLabModel)
            {
                var model = (ScheduleLabModel)validationContext.ObjectInstance;

                if (model != null)
                {
                    var context = (SmarterlabsContext)validationContext.GetService(typeof(SmarterlabsContext));
                    var lab = context.Labs.Where(p => p.Id == (model.LabId ?? 0)).FirstOrDefault();
                    
                    if (lab != null)
                        return model.StartTime > lab.StartDate && model.EndTime < lab.StopDate ? ValidationResult.Success : new ValidationResult("Invalid Schedule Time");
  
                   
                }
            }

            return new ValidationResult("Invalid Schedule time");
        }
    }
}
