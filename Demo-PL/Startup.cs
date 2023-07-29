using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Demo_PL.Mapping_Profiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_PL
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
            services.AddControllersWithViews();  //MVC services
            services.AddDbContext<MVCAppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefultConnection"));
            }/*,ServiceLifetime.Scoped*/);

            //services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            //services.AddSingleton<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>(); 
            services.AddAutoMapper(m => m.AddProfile(new EmployeesProfile()));
            services.AddAutoMapper(m => m.AddProfile(new DepartmentsProfile()));
            services.AddAutoMapper(m => m.AddProfile(new UserProfile()));
            services.AddAutoMapper(m => m.AddProfile(new RoleProfile()));

            services.AddIdentity<ApplicationUsers,IdentityRole>(options=>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                //options.Password.RequiredLength = 5;
            })
                .AddEntityFrameworkStores<MVCAppDbContext>()
                .AddDefaultTokenProviders();

            

            //services.AddScoped<UserManager<ApplicationUsers>>();
            //services.AddScoped<SignInManager<ApplicationUsers>>();
            //services.AddScoped<RoleManager<IdentityRole>>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "Account/Login";  //لو ال login مش مظبوط
					options.AccessDeniedPath= "Home/Error";  //لو مظبوط بس مش authorized

				});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
