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
    
    public partial class CourseDTO 
    {
        public Nullable<int> Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> InstitutionId { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public string CreatedById { get; set; }
        public Nullable<int> SystemStatusId { get; set; }
    }
}