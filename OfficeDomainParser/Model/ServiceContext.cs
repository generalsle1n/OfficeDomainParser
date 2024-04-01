using Microsoft.EntityFrameworkCore;

namespace OfficeDomainParser;

public class ServiceContext : DbContext
{
    public ServiceContext(DbContextOptions options) : base(options)
    {
    }
    DbSet<SingleService> Services { get; set; }
}
