using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Enumerations
{
    public enum SystemStatus
    {
        Active =1,
        Inactive =2,
        Deleted =3
    }

    public enum UserType
    {
        Administrator = 1,
        Student = 2,
        Lecturer = 3,
        Vistor = 4
    }

    public enum InstitutionType
    {
        University = 2,
        HighSchool = 3,
        NGO = 4,
        Internal = 5
    }

    public enum SmarterLabs
    {
        Id = 1
    }

    public enum ScheduleStatus
    {
        Pending = 1,
        Completed = 2,
        Missed = 3,
        Canceled = 4
    }
}
