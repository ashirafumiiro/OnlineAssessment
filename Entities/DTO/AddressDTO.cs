//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.DTO
{
    using System;
    using System.Collections.Generic;
    
    public partial class AddressDTO 
    {
        public Nullable<int> Id { get; set; }
        public string Country { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public string CreatedById { get; set; }
    }
}
