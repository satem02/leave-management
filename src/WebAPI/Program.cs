using LeaveManagement.Infrastructure.Identity;
using LeaveManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using SerilogLog = Serilog.Log;
using Serilog.Core;

namespace LeaveManagement.WebAPI
{
    public class Program
    {
        private static readonly LoggingLevelSwitch LevelSwitch = new LoggingLevelSwitch();
        public async static Task Main(string[] args)
        {
            SerilogLog.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(LevelSwitch)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    "logs/log.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    rollOnFileSizeLimit: true,
                    shared: true)
                .CreateLogger();

            try
            {

                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        var context = services.GetRequiredService<ApplicationDbContext>();
                        context.Database.Migrate();

                        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                        await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager);
                        await ApplicationDbContextSeed.SeedSampleDataAsync(context);
                    }
                    catch (Exception exp)
                    {
                        SerilogLog.Error(exp, "An error occurred while migrating or seeding the database.");
                    }
                }

                await host.RunAsync();
            }
            catch (System.Exception exp)
            {
                SerilogLog.Fatal(exp, "Host terminated unexpectedly");
            }
            finally
            {
                SerilogLog.Information("Ending the web host!");
                SerilogLog.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)                    
                .ConfigureLogging((hostingContext, config) => { config.ClearProviders(); })
                .ConfigureServices(s => { s.AddSingleton(LevelSwitch); })
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}