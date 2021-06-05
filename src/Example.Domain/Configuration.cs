using Example.Domain.Customers;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Domain
{
    public static class Configuration
    {
        public static void AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>();
        }
    }
}