using System.IO;
using Example.Data.Tests.Integration.Customers;
using Example.Data.Tests.Integration.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Data.Tests.Integration
{
    public class DbContextFixture
    {
        public DbContextFixture()
        {
            // Config
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // DI
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ICustomerTestDataFactory, CustomerTestDataFactory>();
            serviceCollection.AddSingleton<IOrderTestDataFactory, OrderTestDataFactory>();
            serviceCollection.AddSingleton<IOrderItemTestDataFactory, OrderItemTestDataFactory>();
            ServiceProvider = serviceCollection.BuildServiceProvider();

            // DbContext
            string dbFilePath = Path.GetFullPath("..\\..\\..\\..\\Example.Data\\DataSource\\ExampleDatabase.mdf");
            string dbConnection = $"{Configuration.GetConnectionString("ExampleDbContext")};AttachDbFilename={dbFilePath}";

            var optionsBuilder = new DbContextOptionsBuilder<ExampleDbContext>()
                .UseSqlServer(dbConnection);
            SetupDbContext = new ExampleDbContext(optionsBuilder.Options);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            VerifyDbContext = new ExampleDbContext(optionsBuilder.Options);
        }

        public IConfiguration Configuration { get; private set; }
        public ServiceProvider ServiceProvider { get; private set; }
        public ExampleDbContext SetupDbContext { get; private set; }
        public ExampleDbContext VerifyDbContext { get; private set; }
    }
}