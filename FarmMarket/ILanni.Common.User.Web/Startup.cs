using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ILanni.Common.User.Web.Data;
using ILanni.Common.User.Web.Models;
using ILanni.Common.User.Web.Services;
using ILanni.Common.User.Repository;
using Pomelo.EntityFrameworkCore.MySql;
using AutoMapper;

namespace ILanni.Common.User.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Mapper.Initialize(cfg => cfg.AddProfiles("ILanni.Common.Identity"));
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ILanni.Common.Identity.User, ILanni.Common.Identity.Role>()
                .AddUserStore<ILanni.Common.Identity.UserStore>()
                .AddRoleStore<ILanni.Common.Identity.RoleStore>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(ops =>
            {
                ops.Password.RequireDigit = false;
                ops.Password.RequiredLength = 6;
                ops.Password.RequiredUniqueChars = 1;
                ops.Password.RequireLowercase = false;
                ops.Password.RequireNonAlphanumeric = false;
                ops.Password.RequireUppercase = false;

            });

            //services.AddAuthentication().AddOAuth()

            services.ConfigureApplicationCookie(ops =>
            {
                
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<UserRepository, UserRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseStatusCodePagesWithRedirects("~/Home/Error{0}");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
