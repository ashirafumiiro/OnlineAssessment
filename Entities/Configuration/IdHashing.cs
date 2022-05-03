using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configuration
{
    public class IdHashing
    {
        public string Salt { get; set; }
        public int MinimumLength { get; set; }
    }
}
