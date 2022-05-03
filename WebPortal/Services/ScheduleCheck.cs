using FluentScheduler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DataAccess.Context;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebPortal.Services
{
    public class ScheduleCheck
    {
        private readonly string checkUrl;
        private readonly ILogger<ScheduleCheck> logger;
        HttpClient client = new HttpClient();

        public ScheduleCheck(IConfiguration config, ILogger<ScheduleCheck> logger)
        {
            this.checkUrl = config["ScheduleCheck:Url"];
            this.logger = logger;
            JobManager.Initialize();

        }
        public void StartJobs()
        {
            JobManager.AddJob(async () => await CheckSchedules(), (s) => s.ToRunEvery(20).Seconds());
        }

        private async Task CheckSchedules()
        {
            logger.LogInformation("Checking Schedules...");
            try
            {
                using (client = new HttpClient())
                {
                    //HttpResponseMessage response = await client.GetAsync(checkUrl);
                    //response.EnsureSuccessStatusCode();
                    //string responseBody = await response.Content.ReadAsStringAsync();
                    // Above three lines can be replaced with new helper method below
                     string responseBody = await client.GetStringAsync(checkUrl);

                    Console.WriteLine(responseBody);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            


        }
    }
}
