using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CloudCare.Data.DbContexts
{
    public class CloudCareContextFactory : IDesignTimeDbContextFactory<CloudCareContext>
    {
        public CloudCareContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CloudCareContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=cloudcare;Username=postgres;Password=postgres");

            return new CloudCareContext(optionsBuilder.Options);
        }
    }
}
