using DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Enities
{
    public partial class Institution
    {
        [NotMapped]
        public string Slug
        {
            get => this.Id.ToSlug();
        }
    }
}
