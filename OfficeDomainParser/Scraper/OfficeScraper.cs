using System.Net;
using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OfficeDomainParser;

public class OfficeScraper : IHostedService
{
    private readonly ILogger<OfficeScraper> _logger;
    private readonly IConfiguration _config;
    private string _url;
    private Guid _guid;
    private HttpClient _http = new HttpClient();
    private IMapper _mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<SingleOfficeRaw, SingleService>();
        }).CreateMapper();
    public OfficeScraper(ILogger<OfficeScraper> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;

        _url = _config.GetValue<string>("OfficeUrl");
        _guid = _config.GetValue<Guid>("Guid");
    }

    internal async Task<List<SingleService>> GetData(CancellationToken Token)
    {
        try
        {
            HttpResponseMessage Reponse = await _http.GetAsync($"{_url}={_guid}", Token);
            _logger.LogInformation($"Read URL {_url}={_guid}");
            string Data = await Reponse.Content.ReadAsStringAsync();
            return ParseData(Data);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error by accessing URL {_url}={_guid}");
            return null;
        }
    }

    private List<SingleService> ParseData(string Data)
    {
        List<SingleOfficeRaw> Parsed = JsonSerializer.Deserialize<List<SingleOfficeRaw>>(Data);
        List<SingleService> Result = new List<SingleService>();
        foreach (SingleOfficeRaw Raw in Parsed)
        {
            SingleService SingleResult = new SingleService
            {
                ID = Guid.NewGuid(),
                Service = Raw.Service,
                DisplayName = Raw.DisplayName,
                TCPPort = new List<int>(),
                UDPPort = new List<int>(),
                URLs = new List<SingleUri>(),
                IPs = new List<SingleIP>()
            };

            if (Raw.TCPPorts is not null)
            {
                int[] Ports = Array.ConvertAll(Raw.TCPPorts.Split(","), int.Parse);
                SingleResult.TCPPort.AddRange(Ports);
            }

            if (Raw.UDPPorts is not null)
            {
                int[] Ports = Array.ConvertAll(Raw.UDPPorts.Split(","), int.Parse);
                SingleResult.UDPPort.AddRange(Ports);
            }

            if (Raw.IPs is not null)
            {
                foreach (string IP in Raw.IPs)
                {
                    SingleIP SingleIP = new SingleIP()
                    {
                        ID = Guid.NewGuid(),
                        IP = IP
                    };

                    SingleResult.IPs.Add(SingleIP);
                }
            }

            if (Raw.Urls is not null)
            {
                foreach (string Url in Raw.Urls)
                {
                    SingleUri SingleUri = new SingleUri()
                    {
                        ID = Guid.NewGuid(),
                        Url = Url
                    };

                    SingleResult.URLs.Add(SingleUri);
                }
            }


            Result.Add(SingleResult);
        }
        return Result;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
