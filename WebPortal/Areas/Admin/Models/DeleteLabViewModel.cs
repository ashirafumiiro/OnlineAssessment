using System.ComponentModel.DataAnnotations;

namespace WebPortal.Areas.Admin.Models
{
    public class DeleteLabViewModel
    {
        [Required]
        public int? Id { get; set; }
    }
}
