using Ecomerce.Infrastructure.Data.EcomerceData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomerce.Infrastructure
{
    public class EcomerceDbContextFactory : IDesignTimeDbContextFactory<EcomerceDbContext>
    {
        public EcomerceDbContext CreateDbContext(string[] args)
        {
            // Read configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // important
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<EcomerceDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new EcomerceDbContext(optionsBuilder.Options);
        }
    }
}
