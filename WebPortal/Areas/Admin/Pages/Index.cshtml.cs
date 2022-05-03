using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebPortal.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        public string Name { get; set; } = "Ashirafu Miiro";
        public void OnGet()
        {
        }
    }
}
