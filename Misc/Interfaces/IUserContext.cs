using DataAccess.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misc.Interfaces
{
    public interface IUserContext
    {
        AspNetUser GetCurrentUser(string includeProperties);
        bool IsInRole(string role);
    }
}
