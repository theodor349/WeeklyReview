using Functions.Configuration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using shared;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((context, config) =>
    {
        // Get the environment (e.g., Development, Production)
        var env = context.HostingEnvironment.EnvironmentName;

        // Load the default appsettings.json
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        // Conditionally load the environment-specific appsettings file
        config.AddJsonFile($"appsettings.{env.ToLower()}.json", optional: false, reloadOnChange: true);

        // Load environment variables
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSharedServices();
        services.AddDatabases(configuration);
    })
    .Build();

host.Run();