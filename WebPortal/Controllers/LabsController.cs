using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DataAccess.Context;
using DataAccess.Enities;
using DataAccess.Extensions;
using Misc.Interfaces;
using Syncfusion.EJ2.Base;
using System.Linq;
using System.Threading.Tasks;
using WebPortal.Areas.Admin.Models;
using WebPortal.Models;

namespace WebPortal.Controllers
{

    [Route("api/[controller]/[action]")]
    public class LabsController : Controller
    {
        private const string AuthSchemes =
        CookieAuthenticationDefaults.AuthenticationScheme + "," +
        JwtBearerDefaults.AuthenticationScheme;

        private readonly ILogger logger;
        private readonly SmarterlabsContext context;
        private readonly IUserContext userContext;

        public LabsController(ILogger<LabsController> logger, SmarterlabsContext context, IUserContext userContext)
        {
            this.logger = logger;
            this.context = context;
            this.userContext = userContext;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        //[Authorize(AuthenticationSchemes = AuthSchemes)]
        public async Task<IActionResult> StudentLabs([FromBody] DataManagerRequest dm)
        {
            var user = userContext.GetCurrentUser("");
            var userInstitutionId = user.InstitutionId;

            var DataSource = context.Labs.GetActive().Where(p => (p.InstitutionId == userInstitutionId || p.InstitutionId == (int)SM.Enumerations.SmarterLabs.Id)).AsQueryable();

            DataOperations operation = new DataOperations();

            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<Lab>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? await Task.FromResult(Json(new { result = DataSource, count = count })) : await Task.FromResult(Json(DataSource));
        }


        [HttpPost]
        public async Task<IActionResult> DeleteLab([FromBody] DeleteLabViewModel dm)
        {
            var operationResult = new OperationResult();
            try
            {
                if (ModelState.IsValid)
                {
                    var lab = await context.Labs.FindAsync(dm.Id);

                    if (lab != null)
                    {
                        lab.SystemStatusId = (int)SM.Enumerations.SystemStatus.Deleted;
                        context.Labs.Update(lab);
                        await context.SaveChangesAsync();
                        operationResult.Message = "<p>Successfully Deleted Lab<p>";
                        operationResult.IsSuccessful = true;
                    }
                    else
                    {
                        this.ModelState.AddModelError("", "Lab Not Found");
                    }
                }
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, ex.Message);
                this.ModelState.AddModelError("", "An error occured while deleting using");
            }

            return PartialView("OperationResult", operationResult);

        }
    }
}
