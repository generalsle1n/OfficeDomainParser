using System.ComponentModel.DataAnnotations;
using System.Net;

namespace OfficeDomainParser;

public class SingleIP
{
    [Key]
    public Guid ID { get; set; }
    public string IP { get; set; }
}
