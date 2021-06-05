using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Example.Data.Customers;
using Example.Data.Orders;
using Example.Data.Tests.Integration.Orders;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Data.Tests.Integration.Customers
{
    public class CustomerTests : IClassFixture<DbContextFixture>
    {
        private readonly ICustomerTestDataFactory _customerTestDataFactory;
        private readonly IOrderTestDataFactory _orderTestDataFactory;
        private readonly ExampleDbContext _setupDbContext;
        private readonly ExampleDbContext _verifyDbContext;

        public CustomerTests(DbContextFixture dbContextFixture)
        {
            _customerTestDataFactory = dbContextFixture.ServiceProvider.GetService<ICustomerTestDataFactory>();
            _orderTestDataFactory = dbContextFixture.ServiceProvider.GetService<IOrderTestDataFactory>();
            _setupDbContext = dbContextFixture.SetupDbContext;
            _verifyDbContext = dbContextFixture.VerifyDbContext;
        }

        [Fact]
        public async Task CanReadWrite()
        {
            Customer setupCustomer = await _customerTestDataFactory.CreateAndAddToContextAsync(_setupDbContext);
            await _setupDbContext.SaveChangesAsync();

            try
            {
                var verifyCustomer = await _verifyDbContext
                    .Customers
                    .SingleOrDefaultAsync(c => c.Id == setupCustomer.Id);

                verifyCustomer.Should().NotBeNull();
                verifyCustomer.Id.Should().BeGreaterThan(0);
                verifyCustomer.Modified.Should().BeBefore(DateTimeOffset.UtcNow);
                verifyCustomer.ModifiedBy.Should().NotBeNull();
                verifyCustomer.Name.Should().Be(setupCustomer.Name);
                verifyCustomer.Address.Should().Be(setupCustomer.Address);
                verifyCustomer.City.Should().Be(setupCustomer.City);
                verifyCustomer.State.Should().Be(setupCustomer.State);
                verifyCustomer.PostalCode.Should().Be(setupCustomer.PostalCode);
            }
            finally
            {
                _setupDbContext.Customers.Remove(setupCustomer);
                await _setupDbContext.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task AppliesIsDeletedQueryFilter()
        {
            var setupCustomer = await _customerTestDataFactory.CreateAndAddToContextAsync(_setupDbContext);
            setupCustomer.IsDeleted = true;
            await _setupDbContext.SaveChangesAsync();

            try
            {
                var verifyCustomer = await _verifyDbContext
                    .Customers
                    .SingleOrDefaultAsync(c => c.Id == setupCustomer.Id);

                verifyCustomer.Should().BeNull();
            }
            finally
            {
                _setupDbContext.Customers.Remove(setupCustomer);
                await _setupDbContext.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task CanNavigateToOrders()
        {
            var setupCustomer = await _customerTestDataFactory.CreateAndAddToContextAsync(_setupDbContext);
            var setupOrders = new List<Order>
            {
                await _orderTestDataFactory.CreateAndAddToContextAsync(setupCustomer, _setupDbContext),
                await _orderTestDataFactory.CreateAndAddToContextAsync(setupCustomer, _setupDbContext)
            };

            await _setupDbContext.SaveChangesAsync();

            try
            {
                Customer verifyCustomer = await _verifyDbContext
                    .Customers
                    .Include(c => c.Orders)
                    .SingleOrDefaultAsync(c => c.Id == setupCustomer.Id);

                verifyCustomer.Should().NotBeNull();
                verifyCustomer.Orders.Should().NotBeNull();
                verifyCustomer.Orders.Count.Should().Be(setupOrders.Count);
            }
            finally
            {
                _setupDbContext.Orders.RemoveRange(setupOrders);
                _setupDbContext.Customers.Remove(setupCustomer);
                await _setupDbContext.SaveChangesAsync();
            }
        }
    }
}