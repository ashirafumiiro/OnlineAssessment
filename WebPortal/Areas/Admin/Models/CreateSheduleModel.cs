using System;

namespace WebPortal.Areas.Admin.Models
{
    public class CreateSheduleModel
    {
        public int LabId { get; set; }
        public string LabName { get; set; }
        public int TimeInterval { get; set; }
        public DateTime MaximumDate { get; set; }
    }
}
