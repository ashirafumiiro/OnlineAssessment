using System.ComponentModel.DataAnnotations;

namespace WebPortal.Areas.Admin.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required, MinLength(3)]
        public string Name { get; set; }
    }
}
