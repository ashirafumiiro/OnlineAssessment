using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using DataAccess.Context;
using Misc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPortal.Configuration;
using WebPortal.Data;
using WebPortal.Services;

namespace WebPortal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SmarterlabsContext>
               (options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services .AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            //                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            // configure strongly typed settings objects
            var jwtSection = Configuration.GetSection("JwtBearerTokenSettings");
            services.Configure<JwtBearerTokenSettings>(jwtSection);
            var jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            services.AddAuthentication(//options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //}
            )
            .AddCookie()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtBearerTokenSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtBearerTokenSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(options =>
            {
                // Create policies
                options.AddPolicy("Admin", p => p.RequireRole(new List<string>() { "Admin", "InstitutionAdmin" }));
            });

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeAreaFolder("Admin", "/", "Admin");
                options.Conventions.AuthorizeAreaFolder("Students", "/");
            });
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver(); // removes camelCase formating default for json
            })
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc); ;

            //services.AddAutoMapper();
            services.AddHttpContextAccessor();
            services.AddTransient<IUserContext, HttpUserContext>();
            services.AddTransient<ScheduleCheck>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ScheduleCheck scheduleCheck)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            string syncfusionLicense = Configuration["Syncfusion:License"]; //add config appropriately
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            scheduleCheck.StartJobs();
        }
    }
}
