using System;
using System.ComponentModel.DataAnnotations;
using WebPortal.Attributes;

namespace WebPortal.Areas.Students.Models
{
    public class ScheduleLabModel
    {
        [Required]
        public int? LabId { get; set; }
        [Required]
        [ExistingScheduleValidator]
        [ScheduleTimeValidator]
        public DateTime? StartTime { get; set; }
        [Required]
        public DateTime? EndTime { get; set; }
    }
}
