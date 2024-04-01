using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OfficeDomainParser;
using Serilog;

const string SettingsFileName = "settings.json";

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Debug()
    .CreateLogger();

IConfigurationRoot Config = new ConfigurationBuilder()
    .AddJsonFile(SettingsFileName, false, true)
    .Build();

var Builder = new HostBuilder()
    .UseSerilog()
    .UseSystemd()
    .UseWindowsService()
    .ConfigureAppConfiguration((config) =>
    {
        config.AddJsonFile(SettingsFileName, false, true);
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<OfficeScraper>();
        services.AddHostedService<DataScraper>();
        services.AddDbContext<ServiceContext>(option =>
        {
            option.UseSqlite($"Data Source={Config.GetValue<string>("DatabaseName")}");

        });
    });

IHost Host = Builder.Build();
await Host.RunAsync();