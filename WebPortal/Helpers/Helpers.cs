using WebPortal.Models;

namespace WebPortal.Helpers
{
    public static class Helpers
    {
        public static SlugDecryptionResponse GetSlugResponse(string slug)
        {
            var slugResponseHelper = new SlugHelper();
            var slugResponse = slugResponseHelper.GetResponse(slug);
            return slugResponse;
        }
    }
}
