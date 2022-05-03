using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DataAccess.Context;
using DataAccess.Enities;
using Syncfusion.EJ2.Base;
using Syncfusion.EJ2.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPortal.Controllers
{
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        private readonly ILogger<DataController> logger;
        private readonly SmarterlabsContext context;
        private readonly RoleManager<IdentityRole> roleManager;

        public DataController(ILogger<DataController> logger, SmarterlabsContext context, RoleManager<IdentityRole> roleManager)
        {
            this.logger = logger;
            this.context = context;
            this.roleManager = roleManager;
        }

        [HttpPost]
        [Route("getLabs")]
        public async Task<IActionResult> GetLabs([FromBody] DataManagerRequest dm)
        {
            var DataSource = this.context.Labs.AsQueryable();
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
            return dm.RequiresCounts ? await Task.FromResult( Json(new { result = DataSource, count = count })) : await Task.FromResult(Json(DataSource));
        }

        [HttpPost]
        [Route("getInstitutions")]
        public async Task<IActionResult> GetInstitutions([FromBody] DataManagerRequest dm)
        {
            var DataSource = this.context.Institutions
                //.Include(i => i.Address)
                //.Include(i => i.Type)
                .AsEnumerable();
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
            int count = DataSource.Cast<Institution>().Count();
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
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers([FromBody] DataManagerRequest dm)
        {
            var DataSource = context.AspNetUsers.AsQueryable();

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
            int count = DataSource.Cast<AspNetUser>().Count();
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
    }
}
