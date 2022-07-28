using Microsoft.EntityFrameworkCore;

namespace IdealEFCoreConfigs.Model;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
}