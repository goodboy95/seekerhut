using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using NLog.Extensions.Logging;
using Dao;
using Middleware;
using Utils;
using System.IO;
using Pomelo.EntityFrameworkCore.MySql;
using StackExchange.Redis;
using System;

namespace web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(options => {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            services.AddDbContext<DwDbContext>(options => options.UseMySql(Configuration.GetConnectionString("mysql")));
            //services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DwDbContext c)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            loggerFactory.AddNLog().AddDebug();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
            }
            //app.UseSession();
            app.UseWebSockets();
            //IConfigurationSection redisConf = Configuration.GetSection("Redis");
            //StackRedisHelper.InitRedis(redisConf.GetValue<string>("ConnStr"), redisConf.GetValue<Int32>("Database"), redisConf.GetValue<string>("Name"));
            //如/file对应的文件夹不存在，自动创建文件夹
            if (!Directory.Exists(Configuration.GetSection("VirtualPath").GetValue<string>("File")))
            {
                Directory.CreateDirectory(Configuration.GetSection("VirtualPath").GetValue<string>("File"));
            }
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(Configuration.GetSection("VirtualPath").GetValue<string>("File")),
                RequestPath = "/file",
                EnableDirectoryBrowsing = false
            });
            
            app.UseStaticFiles();
            Initialize.DbInit(c);
            //app.UseErrorPage();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
