using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetworkSocket;
using NetworkSocket.Fast;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace WebApi
{
    public class Startup
    {
        /// <summary>
        /// 设备后台监听
        /// </summary>
        private TcpListener backgroundListener;
        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .AddEnvironmentVariables().Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddFastService();
            //增加单例双向通讯TCP
            backgroundListener = new TcpListener();
            services.AddSingleton(backgroundListener);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            #region 绑定Fast
            //端口号
            int fastPort = Convert.ToInt32(Configuration["FASTPORT"]);
            //将注入容器传入Client，注册所有fastApi
            backgroundListener.UseProvider(app.ApplicationServices);
            //var fastFilter = app.ApplicationServices.GetRequiredService<ILogger<FastGlobalFilter>>();
            backgroundListener.Use<FastMiddleware>().GlobalFilters.Add(new FastGlobalFilter());
            var cert = new X509Certificate2($"cert/openssl.pfx", "123456");
            backgroundListener.UseSSL(cert);
            backgroundListener.Start(fastPort);
            #endregion
        }
    }
}
