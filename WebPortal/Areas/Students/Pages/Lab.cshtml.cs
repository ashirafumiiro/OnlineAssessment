using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Extensions;
using System.Threading.Tasks;
using WebPortal.Areas.Students.Models;
using WebPortal.Models;

namespace WebPortal.Areas.Students.Pages
{
    public class LabModel : PageModel
    {
        private readonly SmarterlabsContext context;

        public LabModel(SmarterlabsContext context)
        {
            this.context = context;
        }

        public LabViewModel Lab { get; set; }
        public async Task<IActionResult> OnGet(string viewId)
        {
            var slugResponse = Helpers.Helpers.GetSlugResponse(viewId);

            if (string.IsNullOrEmpty(viewId) || slugResponse.Status != SlugResponseStatus.Existing)
            {
                return NotFound();
            }

            var labToView = await context.Labs.FirstOrDefaultAsync(m => m.Id == slugResponse.Id);

            if (labToView == null)
            {
                return NotFound();
            }

            Lab = new LabViewModel()
            {
                Id = labToView.Id.ToSlug(),
                Name = labToView.Name,
                Description = labToView.Description
            };


            return Page(); 
        }
    }
}
