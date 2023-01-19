namespace MyStore.API
{
    using MyStore.API.Commons;
    using MyStore.Infrastructure.Persistence;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Events;
    using System;
    using System.IO;

    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = CreateLoggerConfiguration(configuration);

            try
            {
                Log.Information("Starting web host");

                var host = CreateHostBuilder(args).Build();

                CreateDbIfNotExists(host);

                host.Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<SLADbContext>();
                DbInitializer.Initialize(context);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An error occurred creating the DB.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                           webBuilder.UseStartup<Startup>().UseSerilog();
                       });
        }

        private static Serilog.ILogger CreateLoggerConfiguration(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.MSSqlServer(configuration.GetConnectionString("Logger"), "EventLog", autoCreateSqlTable: true)
                .CreateLogger();
        }

        private static IConfiguration GetConfiguration()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            return new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .Build();
        }
    }
}
