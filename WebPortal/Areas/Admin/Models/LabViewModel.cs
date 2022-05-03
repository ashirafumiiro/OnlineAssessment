using System;
using System.ComponentModel.DataAnnotations;

namespace WebPortal.Areas.Admin.Models
{
    public class LabViewModel
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Token { get; set; }
        [Display(Name = "Token Expiration")]
        public DateTime? TokenExpiration { get; set; }
        [Required]
        [Display(Name ="Start Date")]
        public DateTime? StartDate { get; set; }
        [Required]
        [Display(Name = "Stop Date")]
        public DateTime? StopDate { get; set; }
        [Required]
        [Display(Name = "Maximum Time")]
        public int MaximumTime { get; set; }
        [Required]
        public int? InstitutionId { get; set; }

        public string Description { get; set; }
    }
}
