using Contacts.Persistence.Extensions;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers.Span;

namespace Contacts.WebApi.Infrastructure.Extensions;

public static class WebApplicationExtensions
{
    public static int RunHost(this WebApplication webApplication, string[] args)
    {
        LoggerConfiguration loggerConfig = ConfigureLogger(args);

        using Logger logger = loggerConfig.CreateLogger();

        Log.Logger = logger.ForContext(typeof(Log));

        try
        {
            logger.Information("Starting host");

            webApplication.Services.RunMigration();
            webApplication.Run();

            logger.Information("Host successfully stopped");

            return 0;
        }
        catch (Exception ex)
        {
            logger.Fatal(ex, "Host terminated unexpectedly");
            return 1;
        }
    }

    private static IConfiguration GetLoggerConfiguration(string[] args)
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json",
                true,
                true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();
    }

    private static LoggerConfiguration ConfigureLogger(string[] args)
    {
        LoggerConfiguration loggerConfig = new LoggerConfiguration().Enrich.WithSpan();

        return loggerConfig.ReadFrom.Configuration(GetLoggerConfiguration(args));
    }
}