using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Example.Data.Customers;
using Example.Data.Orders;
using Example.Data.Tests.Integration.Customers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Example.Data.Tests.Integration.Orders
{
    public class OrderTests : IClassFixture<DbContextFixture>
    {
        private readonly ICustomerTestDataFactory _customerTestDataFactory;
        private readonly IOrderTestDataFactory _orderTestDataFactory;
        private readonly IOrderItemTestDataFactory _orderItemTestDataFactory;
        private readonly ExampleDbContext _setupDbContext;
        private readonly ExampleDbContext _verifyDbContext;

        public OrderTests(DbContextFixture dbContextFixture)
        {
            _customerTestDataFactory = dbContextFixture.ServiceProvider.GetService<ICustomerTestDataFactory>();
            _orderTestDataFactory = dbContextFixture.ServiceProvider.GetService<IOrderTestDataFactory>();
            _orderItemTestDataFactory = dbContextFixture.ServiceProvider.GetService<IOrderItemTestDataFactory>();
            _setupDbContext = dbContextFixture.SetupDbContext;
            _verifyDbContext = dbContextFixture.VerifyDbContext;
        }

        [Fact]
        public async Task CanReadWrite()
        {
            Customer setupCustomer = await _customerTestDataFactory.CreateAndAddToContextAsync(_setupDbContext);
            Order setupOrder = await _orderTestDataFactory.CreateAndAddToContextAsync(setupCustomer, _setupDbContext);
            await _setupDbContext.SaveChangesAsync();

            try
            {
                Order verifyOrder = await _verifyDbContext
                    .Orders
                    .Include(o => o.Customer)
                    .SingleOrDefaultAsync(o => o.Id == setupOrder.Id);

                verifyOrder.Should().NotBeNull();
                verifyOrder.Customer.Should().NotBeNull();
                verifyOrder.Customer.Id.Should().Be(setupCustomer.Id);
                verifyOrder.Modified.Should().BeBefore(DateTimeOffset.UtcNow);
                verifyOrder.ModifiedBy.Should().NotBeNull();
                verifyOrder.OrderNumber.Should().Be(setupOrder.OrderNumber);
            }
            finally
            {
                _setupDbContext.Remove(setupOrder);
                _setupDbContext.Remove(setupCustomer);
                await _setupDbContext.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task AppliesQueryFilter()
        {
            Customer setupCustomer = await _customerTestDataFactory.CreateAndAddToContextAsync(_setupDbContext);
            Order setupOrder = await _orderTestDataFactory.CreateAndAddToContextAsync(setupCustomer, _setupDbContext);
            setupOrder.IsDeleted = true;
            await _setupDbContext.SaveChangesAsync();

            try
            {
                Order verifyOrder = await _verifyDbContext
                    .Orders
                    .Include(o => o.Customer)
                    .SingleOrDefaultAsync(o => o.Id == setupOrder.Id);

                verifyOrder.Should().BeNull();
            }
            finally
            {
                _setupDbContext.Remove(setupOrder);
                _setupDbContext.Remove(setupCustomer);
                await _setupDbContext.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task CanNavigateToOrderItems()
        {
            Customer setupCustomer = await _customerTestDataFactory.CreateAndAddToContextAsync(_setupDbContext);
            Order setupOrder = await _orderTestDataFactory.CreateAndAddToContextAsync(setupCustomer, _setupDbContext);
            var setupOrderItems = new List<OrderItem>
            {
                await _orderItemTestDataFactory.CreateAndAddToContextAsync(setupOrder, _setupDbContext),
                await _orderItemTestDataFactory.CreateAndAddToContextAsync(setupOrder, _setupDbContext)
            };
            await _setupDbContext.SaveChangesAsync();

            try
            {
                Order verifyOrder = await _verifyDbContext
                    .Orders
                    .Include(o => o.OrderItems)
                    .SingleOrDefaultAsync(o => o.Id == setupOrder.Id);

                verifyOrder.Should().NotBeNull();
                verifyOrder.OrderItems.Should().NotBeNull();
                verifyOrder.OrderItems.Count.Should().Be(setupOrderItems.Count);
            }
            finally
            {
                _setupDbContext.Remove(setupOrder);
                _setupDbContext.Remove(setupCustomer);
                _setupDbContext.RemoveRange(setupOrderItems);
                await _setupDbContext.SaveChangesAsync();
            }
        }
    }
}
