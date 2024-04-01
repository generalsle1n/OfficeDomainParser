using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace OfficeDomainParser;

public class SingleService
{
    [Key]
    public Guid ID { get; set; }
    public string Service { get; set; }
    public string DisplayName { get; set; }
    public List<SingleUri> URLs { get; set; }
    public List<SingleIP> IPs { get; set; }
    public List<int> TCPPort { get; set; }
    public List<int> UDPPort { get; set; }
}