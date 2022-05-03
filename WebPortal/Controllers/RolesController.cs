using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DataAccess.Context;
using System;
using Syncfusion.EJ2.Base;
using Syncfusion.EJ2.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPortal.Areas.Admin.Models;
using WebPortal.Models;

namespace WebPortal.Controllers
{
    [Route("api/[controller]/[action]")]
    public class RolesController : Controller
    {
        private readonly ILogger<DataController> logger;
        private readonly SmarterlabsContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public RolesController(ILogger<DataController> logger, SmarterlabsContext context, RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            this.logger = logger;
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        [HttpPost]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles([FromBody] DataManagerRequest dm)
        {
            var DataSource = roleManager.Roles;

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
            } int count = DataSource.Cast<IdentityRole>().Count();
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

        [HttpGet]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var roles = await userManager.GetRolesAsync(user);

            var rolesItems = roles.Select(p =>
                new { Name = p }
            );


            return Json(new { result = rolesItems, count = rolesItems.Count() });
        }

        [HttpPost]
        [Route("ApplyRole")]
        public async Task<IActionResult> ApplyRole([FromBody] ApplyRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByNameAsync(model.RoleName);
                if (role == null)
                {
                    this.ModelState.AddModelError("", "Role Not found");
                }
                try
                {
                    var identityUser = await userManager.FindByIdAsync(model.UserId);
                    var result = await userManager.AddToRoleAsync(identityUser, role.Name);

                    if (result.Succeeded)
                    {
                        return Json(new { status = "OK", message = "Role Added successfully" });
                    }
                    else
                    {
                        throw new Exception(result.Errors.FirstOrDefault().Description);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.ToString());
                    this.ModelState.AddModelError("", $"An Errror occured occured while applying role. Message: {ex.Message}");
                }
            }

            return PartialView("~/Areas/Admin/Pages/Shared/ModelErrors.cshtml");
        }

        [HttpGet]
        public IActionResult RolesDropdownList()
        {
            var allRoles = roleManager.Roles;

            var rolesItems = allRoles.Select(p =>
                new
                {
                    Name = p.Name,
                    Value = p.Name
                }).ToList();

            return Json(rolesItems);
        }

        [HttpPost]
        [Route("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole([FromBody] ApplyRoleModel model)
        {
            var response = new { status = "Error", message = "An Error occured while removing role" };
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByNameAsync(model.RoleName);
                if (role == null)
                {
                    response = new { status = "Error", message = "Role not found" };
                }
                try
                {
                    var identityUser = await userManager.FindByIdAsync(model.UserId);
                    var result = await userManager.RemoveFromRoleAsync(identityUser, role.Name);

                    if (result.Succeeded)
                    {
                        return Json(new { status = "OK", message = "Role Added successfully" });
                    }
                    else
                    {
                        response = new { status = "Error", message = result.Errors.FirstOrDefault().Description };
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.ToString());
                }
            }

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole([FromBody] DeleteRoleModel model)
        {
            var operationResult = new OperationResult();
            try
            {
                if (ModelState.IsValid)
                {
                    var role = await roleManager.FindByIdAsync(model.RoleId);

                    if (role != null)
                    {
                        var result = await roleManager.DeleteAsync(role);
                        if (result.Succeeded)
                        {
                            operationResult.Message = "<p>Successfully Deleted Role<p>";
                            operationResult.IsSuccessful = true;
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                this.ModelState.AddModelError("", error.Description);
                            }
                        }
                        
                    }
                    else
                    {
                        this.ModelState.AddModelError("", "Role Not Found");
                    }
                }
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, ex.Message);
                this.ModelState.AddModelError("", "An error occured while deleting role");
            }

            return PartialView("OperationResult", operationResult);

        }

        public async Task<IActionResult> CreateRole([FromBody] RoleViewModel model)
        {
            var operationResult = new OperationResult();
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.Name
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    operationResult.Message = "<p>Successfully Created Role<p>";
                    operationResult.IsSuccessful = true;
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }         

            return PartialView("OperationResult", operationResult);
        }
    }
}
