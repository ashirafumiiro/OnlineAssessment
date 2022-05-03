using HashidsNet;
using DataAccess.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Extensions
{
    public static class Strings
    {
        public static string ToSlug(this int id)
        {
            var settings = AppSettings.GetSMSettings();
            var salt = settings.IdHashing.Salt;
            var hashids = new Hashids(salt, settings.IdHashing.MinimumLength);
            var hash = hashids.Encode(id);
            return hash;
        }

        public static string ToSlug(this long id)
        {
            var settings = AppSettings.GetSMSettings();
            var salt = settings.IdHashing.Salt;
            var hashids = new Hashids(salt, settings.IdHashing.MinimumLength);
            var hash = hashids.Encode((int)id);
            return hash;
        }

    }
}
