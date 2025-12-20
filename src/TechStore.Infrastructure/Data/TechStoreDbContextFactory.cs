using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TechStore.Infrastructure.Data
{
    public class TechStoreDbContextFactory : IDesignTimeDbContextFactory<TechStoreDbContext>
    {
        public TechStoreDbContext CreateDbContext(string[] args)
        {
            // Caminho para o appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "TechStore.API"))
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<TechStoreDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new TechStoreDbContext(optionsBuilder.Options);
        }
    }
}