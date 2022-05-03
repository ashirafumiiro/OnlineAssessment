using System.ComponentModel.DataAnnotations;
using WebPortal.Attributes;

namespace WebPortal.Areas.Admin.Models
{
    public class CancelScheduleModel
    {
        [Required]
        [CanceActiveScheduleValidator]

        public string Id { get; set; }
    }
}
