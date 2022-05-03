using System.ComponentModel.DataAnnotations;

namespace WebPortal.Areas.Admin.Models
{
    public class ApplyRoleModel
    {
        [Required]
        public string RoleName { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
