using System.ComponentModel.DataAnnotations;

namespace WebPortal.Areas.Admin.Models
{
    public class DeleteRoleModel
    {
        [Required]
        public string RoleId { get; set; }
    }
}
