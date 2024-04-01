using System.ComponentModel.DataAnnotations;

namespace OfficeDomainParser;

public class SingleUri
{
    [Key]
    public Guid ID { get; set; }
    public string Url { get; set; }
}
