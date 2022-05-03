using System;
using System.ComponentModel.DataAnnotations;

namespace WebPortal.Areas.Admin.Models
{
    public class InstitutionViewModel
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime Created { get; set; }
        [Required]
        public int TypeId { get; set; }
        public int? AddressId { get; set; }
        public int? CountryId { get; set; }
        public string CreatedById { get; set; }
        public int? SystemStatusId { get; set; }
        public Address Address { get; set; }

    }

    public class Address
    {
        public int? Id { get; set; }
        [Required]

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Country { get; set; }
        
    }
}
