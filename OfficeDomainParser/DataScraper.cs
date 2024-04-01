using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OfficeDomainParser;

public class DataScraper : IHostedService
{
    private readonly ILogger<DataScraper> _logger;
    private readonly IConfiguration _config;
    private const int _timemultiplyer = 60000;
    private OfficeScraper _officeScaper;
    private ServiceContext _db;
    public DataScraper(ILogger<DataScraper> logger, IConfiguration config, OfficeScraper officeScraper, ServiceContext context)
    {
        _logger = logger;
        _config = config;
        _officeScaper = officeScraper;
        _db = context;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _db.Database.EnsureCreatedAsync(cancellationToken);
        while (!cancellationToken.IsCancellationRequested)
        {
            var Data = await _officeScaper.GetData(cancellationToken);
            await _db.AddRangeAsync(Data);
            await _db.SaveChangesAsync();
            await Task.Delay(_config.GetValue<int>("Frequency") * _timemultiplyer);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
