using System.Text.Json.Serialization;

namespace OfficeDomainParser;
public class SingleOfficeRaw
{
    [JsonPropertyName("id")]
    public int ID { get; set; }
    [JsonPropertyName("serviceArea")]
    public string Service { get; set; }
    [JsonPropertyName("serviceAreaDisplayName")]
    public string DisplayName { get; set; }
    [JsonPropertyName("urls")]
    public List<string> Urls { get; set; }
    [JsonPropertyName("ips")]
    public List<string> IPs { get; set; }
    [JsonPropertyName("tcpPorts")]
    public string TCPPorts { get; set; }
    [JsonPropertyName("udpPorts")]
    public string UDPPorts { get; set; }
    [JsonPropertyName("expressRoute")]
    public bool ExpressRoute { get; set; }
    [JsonPropertyName("category")]
    public string Category { get; set; }
    [JsonPropertyName("required")]
    public bool Required { get; set; }
}
