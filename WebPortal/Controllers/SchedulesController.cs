using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DataAccess.Context;
using DataAccess.Enities;
using DataAccess.Extensions;
using DataAccess.Messaging;
using Misc.Interfaces;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebPortal.Areas.Admin.Models;
using WebPortal.Areas.Students.Models;
using WebPortal.Models;

namespace WebPortal.Controllers
{
    [Route("api/[controller]")]
    public class SchedulesController : Controller
    {
        private readonly ILogger logger;
        private readonly SmarterlabsContext context;
        private readonly IUserContext userContext;

        public SchedulesController(ILogger<LabsController> logger, SmarterlabsContext context, IUserContext userContext)
        {
            this.logger = logger;
            this.context = context;
            this.userContext = userContext;
        }

        [Route("GetSchedules")]
        [HttpPost]
        public async Task<IActionResult> GetSchedules([FromBody] DataManagerRequest dm)
        {
            var DataSource = context.Schedules.Include(s => s.User).AsQueryable();

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
            int count = DataSource.Cast<Schedule>().Count();
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
        [Route("GetBlockData")]
        public async Task<IActionResult> GetBlockData([FromBody] PostModel model)
        {
            var data = FilterAppointment(model.LabId, model.StartDate, model.CurrentView);

            return await Task.FromResult(Json(data));
        }


        public List<AppointmentData> FilterAppointment(int labId, DateTime CurrentDate, string CurrentView = "Week")
        {
            if (CurrentDate == default(DateTime))
                CurrentDate = DateTime.Now.ToUniversalTime();
            else
                CurrentDate = CurrentDate.AddDays(1);

            DateTime CurrDate = Convert.ToDateTime(CurrentDate);
            DateTime StartDate = FirstWeekDate(CurrDate.Date);
            DateTime EndDate = FirstWeekDate(CurrDate.Date);

            switch (CurrentView.ToLower())
            {
                case "day":
                    StartDate = CurrentDate;
                    EndDate = CurrentDate.AddHours(24);
                    break;
                case "week":
                    EndDate = EndDate.AddDays(7);
                    break;
                case "workweek":
                    EndDate = EndDate.AddDays(5);
                    break;
                case "month":
                    StartDate = CurrDate.Date.AddDays(-CurrDate.Day + 1);
                    EndDate = StartDate.AddMonths(1);
                    break;
                case "agenda":
                    EndDate = EndDate.AddDays(7);
                    break;
            }
            var defaultScheduleList = GetScheduleData(labId, StartDate, EndDate);// here particular date DefaultSchedule is filtered 
            return defaultScheduleList;
        }

        public List<AppointmentData> GetScheduleData(int labId, DateTime startDate, DateTime endDate)
        {
            var user = userContext.GetCurrentUser("");
            var userId = user.Id;
            var data = context.Schedules.Where(p => p.LabId == labId && p.StartTime >= startDate.ToUniversalTime() && p.EndTime <= endDate.ToUniversalTime() && p.StatusId == (int)SM.Enumerations.ScheduleStatus.Pending
                ).Select(x => new AppointmentData
                { Id = x.Id, Subject = "Not available", StartTime = x.StartTime, EndTime = x.EndTime, IsBlock = true }).ToList();

            return data;
        }

        internal static DateTime FirstWeekDate(DateTime CurrentDate)
        {
            try
            {
                DateTime FirstDayOfWeek = CurrentDate;
                DayOfWeek WeekDay = FirstDayOfWeek.DayOfWeek;
                switch (WeekDay)
                {
                    case DayOfWeek.Sunday:
                        break;
                    case DayOfWeek.Monday:
                        FirstDayOfWeek = FirstDayOfWeek.AddDays(-1);
                        break;
                    case DayOfWeek.Tuesday:
                        FirstDayOfWeek = FirstDayOfWeek.AddDays(-2);
                        break;
                    case DayOfWeek.Wednesday:
                        FirstDayOfWeek = FirstDayOfWeek.AddDays(-3);
                        break;
                    case DayOfWeek.Thursday:
                        FirstDayOfWeek = FirstDayOfWeek.AddDays(-4);
                        break;
                    case DayOfWeek.Friday:
                        FirstDayOfWeek = FirstDayOfWeek.AddDays(-5);
                        break;
                    case DayOfWeek.Saturday:
                        FirstDayOfWeek = FirstDayOfWeek.AddDays(-6);
                        break;
                }
                return (FirstDayOfWeek);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        [Route("ScheduleLab")]
        [HttpPost]
        public async Task<IActionResult> ScheduleLab([FromBody] ScheduleLabModel model)
        {
            var operationResult = new OperationResult();
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime startTime = Convert.ToDateTime(model.StartTime);

                    DateTime endTime = Convert.ToDateTime(model.EndTime);

                    var user = userContext.GetCurrentUser("");

                    if (DateTime.Compare( DateTime.Now,  endTime) < 0 ) {
                        var schedule = new Schedule()
                        {
                            UserId = user.Id,
                            LabId = model.LabId.Value,
                            StartTime = model.StartTime.Value,
                            EndTime = model.EndTime.Value,
                            StatusId = (int)SM.Enumerations.ScheduleStatus.Pending,
                            SystemStatusId = (int)SM.Enumerations.SystemStatus.Active,

                        };
                        context.Schedules.Add(schedule);
                        await context.SaveChangesAsync();

                        operationResult.Message = "<p>Successfully Created Schedule<p>";
                        operationResult.IsSuccessful = true;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Time slot No longer available");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                    ModelState.AddModelError("", "An error occured why performing action");
                }

            }

            return PartialView("OperationResult", operationResult);
        }

        [HttpGet]
        [Route("CheckSchedules")]
        public async Task<IActionResult> CheckSchedules()
        {
            ServiceMessage<List< Schedule >> response = new ServiceMessage<List<Schedule>>();
            
            try
            {
                var now = DateTime.Now.ToUniversalTime();
                var pastSchedules = context.Schedules.Where(p => p.SystemStatusId == (int)SM.Enumerations.SystemStatus.Active)
                    .Where(p => p.EndTime < now  && p.StatusId == (int)SM.Enumerations.ScheduleStatus.Pending).ToList();

                if (pastSchedules.Any())
                {
                    foreach (var schedule in pastSchedules)
                    {
                        schedule.StatusId = (int)SM.Enumerations.ScheduleStatus.Missed;
                        context.Schedules.Update(schedule);
                        await context.SaveChangesAsync();
                    }
                }

                response.Data = pastSchedules;
            }
            catch (Exception ex)
            {
                response.ErrorOrWarningNarrative = ex.Message;
                response.ErrorOrWarningCode = "500";
                logger.LogError(ex, ex.Message);
            }
            return Json(response);
        }

        [HttpPost]
        [Route("CancelSchedules")]
        public async Task<IActionResult> CancelSchedules([FromBody] CancelScheduleModel modal)
        {
            var operationResult = new OperationResult();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = userContext.GetCurrentUser("");
                    var slugResponse = Helpers.Helpers.GetSlugResponse(modal.Id);
                    var scheduleToUpdate = await context.Schedules.GetActive().Where(p => p.Id == slugResponse.Id).FirstOrDefaultAsync();

                    if (slugResponse.Status == SlugResponseStatus.Existing && user != null && scheduleToUpdate != null)
                    {
                        scheduleToUpdate.SystemStatusId = (int)SM.Enumerations.SystemStatus.Deleted;
                        scheduleToUpdate.StatusId = (int)SM.Enumerations.ScheduleStatus.Canceled;

                        context.Schedules.Update(scheduleToUpdate);
                        await context.SaveChangesAsync();

                        operationResult.Message = "<p>Successfully Created Schedule<p>";
                        operationResult.IsSuccessful = true;
                        
                    }
                    else
                    {
                        ModelState.AddModelError("", "Schedule not found");
                    }
                    
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                    ModelState.AddModelError("", "An error occured why performing action");
                }

            }

            return PartialView("OperationResult", operationResult);
        }
    }

    public class AppointmentData
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsBlock { get; set; }
    }

    public class PostModel
    {
        public string CurrentView { get; set; }
        public int LabId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
