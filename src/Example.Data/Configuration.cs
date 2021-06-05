using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Data
{
    public static class Configuration
    {
        public static void AddDataServices(this IServiceCollection services, string dbConnectionString)
        {
            services.AddDbContext<ExampleDbContext>(options =>
                options.UseSqlServer(dbConnectionString));

            services.AddScoped<Domain.Customers.ICustomerRepository, Customers.CustomerRepository>();
        }
    }
}
