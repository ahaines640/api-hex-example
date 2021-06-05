using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Example.Data.Tests
{
    public class DbContextTest
    {
        protected ExampleDbContext SetupDbContext;
        protected ExampleDbContext VerifyDbContext;
        protected IMapper Mapper;
        protected string TestUser = Guid.NewGuid().ToString();

        protected DbContextTest()
        {
            DbContextOptions<ExampleDbContext> options = new DbContextOptionsBuilder<ExampleDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            SetupDbContext = new ExampleDbContext(options);
            SetupDbContext.Database.EnsureCreated();

            VerifyDbContext = new ExampleDbContext(options);
            VerifyDbContext.Database.EnsureCreated();

            Mapper = new MapperConfiguration(cfg => cfg.AddProfile(typeof(DataMappingProfile)))
                .CreateMapper();
        }
    }
}
