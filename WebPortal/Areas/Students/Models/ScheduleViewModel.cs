using DataAccess.Enities;
using System;

namespace WebPortal.Areas.Students.Models
{
    public class ScheduleViewModel
    {
        public string Id { get; set; }
        public string LabName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public SM.Enumerations.ScheduleStatus Status { get; set; }
    }
}
