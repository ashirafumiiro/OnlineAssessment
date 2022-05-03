using System.ComponentModel.DataAnnotations;

namespace WebPortal.Models.ViewModels
{
    public class UpdateAccountViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Institution")]
        public int InstitutionId { get; set; }

        [Required]
        [Display(Name = "Email/UserName")]
        public string Email { get; set; }
    }
}
