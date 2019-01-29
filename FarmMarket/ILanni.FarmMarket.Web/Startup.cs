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
using ILanni.FarmMarket.Web.Data;
using ILanni.FarmMarket.Web.Models;
using ILanni.FarmMarket.Web.Services;
using Nest;
using NLog.Extensions.Logging;
using NLog.Web;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core;
using MongoDB.Driver;
using ILanni.FarmMarket.Domain;
using ILanni.FarmMarket.Repository.Mongo;
using ILanni.FarmMarket.MQ;
using ILanni.Common.RabbitMQ;

namespace ILanni.FarmMarket.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            env.ConfigureNLog("nlog.config");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
            AutoMapper.Mapper.Initialize(c => c.AddProfile<ILanni.FarmMarket.Repository.Mongo.AutoMapperProfile>());
            Elasticsearch.Net.StaticConnectionPool pool = new StaticConnectionPool(new[] { new Uri("http://localhost:9200") });
            ConnectionSettings estSettings = new ConnectionSettings(pool);
            estSettings.BasicAuthentication("elastic", "123456");
            estSettings.InferMappingFor<Models.Product>(d => d.IdProperty(p => p.Id).IndexName("product").TypeName("product"));
            ElasticClient client = new ElasticClient(estSettings);
            services.AddSingleton<ElasticClient>(client);

            MongoClientSettings mSettings = new MongoClientSettings()
            {
                Credential = MongoCredential.CreateCredential("admin", "sa", "1"),
                Server = new MongoServerAddress("127.0.0.1", 27017),
                
            };
            MongoClient mClient = new MongoClient(mSettings);
            services.AddSingleton<MongoClient>(mClient);

            services.AddSingleton<ILanni.FarmMarket.MQ.Settings>(new ILanni.FarmMarket.MQ.Settings());

            var rmqConnFactory = new RabbitMQ.Client.ConnectionFactory()
            {
                UserName = "admin",
                Password = "1"
            };
            services.AddSingleton<RabbitMQ.Client.ConnectionFactory>(rmqConnFactory);

            PoolingSettings mqSetting = new PoolingSettings() { MaxChannelPerConnection = 5, MaxConnections = 2 };
            ChannelPool channelPool = new ChannelPool(rmqConnFactory, mqSetting);
            services.AddSingleton(channelPool);

            services.AddScoped<ProductRepository>();
            services.AddScoped<ProductPublisher>();
            services.AddScoped<ProductService>();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            loggerFactory.AddNLog();

            //add NLog.Web
            app.AddNLogWeb();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
