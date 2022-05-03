using System.ComponentModel.DataAnnotations;

namespace WebPortal.Areas.Admin.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        [Display(Name = "Status")]
        public int SystemStatusId { get; set; }

        [Required]
        [Display(Name = "Institution Type")]
        public int InstitutionId { get; set; }
        [Required]
        [Display(Name = "User Type")]
        public int TypeId { get; set; }

        [Required]
        [Display(Name = "Email/UserName")]
        public string Email { get; set; }
    }
}
