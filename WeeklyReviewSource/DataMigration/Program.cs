using DataMigration.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var myEnv = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(x =>
    {
        x.AddJsonFile($"appsettings.Development.json", false);
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
        services.AddSingleton<IActivityAccess, ActivityAccess>();
        services.AddSingleton<IRecordAccess, RecordAccess>();
        services.AddSingleton<IRecordTimeAccess, RecordTimeAccess>();
        services.AddSingleton<IMigrator, Migrator>();
    })
    .Build();

// code
var migrator = host.Services.GetService<IMigrator>();
await migrator.Migrate();

await host.RunAsync();
