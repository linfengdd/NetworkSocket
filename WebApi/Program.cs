using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
               .Enrich.WithProperty("Fast", "FastHost")
               .Enrich.FromLogContext()
               .WriteTo.Async(a => a.File($"{AppContext.BaseDirectory}Logs/.log",
     retainedFileCountLimit: null, //文件永久保存，默认是31天
     rollingInterval: Serilog.RollingInterval.Day,
     outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} || {Level} || {SourceContext:l} || {Message} || {Exception} ||end {NewLine}"))
               .CreateLogger();
            try
            {
                Log.Information("Starting FastHost.");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "FastHost terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// CreateHostBuilder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
