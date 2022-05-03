using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HashidsNet;
using DataAccess.Configuration;
using DataAccess.Extensions;

namespace WebPortal.Models
{

    public class SlugHelper
    {
        public SlugDecryptionResponse GetResponse(string slug)
        {
            var settings = AppSettings.GetSMSettings();
            var slugResponse = new SlugDecryptionResponse();
            var salt = settings.IdHashing.Salt;
            var hashids = new Hashids(salt, settings.IdHashing.MinimumLength);

            try
            {
                var numbers = hashids.Decode(slug);
                if (numbers.Length > 0)
                {
                    slugResponse.Id = numbers[0];
                    slugResponse.Status = SlugResponseStatus.Existing;
                }
                else
                {
                    slugResponse.Status = SlugResponseStatus.Invalid;
                }
            }
            catch (NoResultException)
            { // Decoding the provided hash has not yielded any result.
                slugResponse.Status = SlugResponseStatus.Invalid;
            }
            catch (MultipleResultsException)
            { // The decoding process yielded more than one result when just one was expected.
                slugResponse.Status = SlugResponseStatus.Invalid;
            }


            return slugResponse;
        }
    }

    public class SlugDecryptionResponse
    {
        public SlugResponseStatus Status { get; set; }
        public int Id { get; set; }
        public Guid Uid { get; set; }
    }


    public enum SlugResponseStatus
    {
        Existing,
        New,
        Invalid
    }

}