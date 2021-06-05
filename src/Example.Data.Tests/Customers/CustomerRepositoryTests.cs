using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Example.Data.Customers;
using FluentAssertions;
using Xunit;

namespace Example.Data.Tests.Customers
{
    public class CustomerRepositoryTests : DbContextTest
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerRepositoryTests()
        {
            _customerRepository = new CustomerRepository(Mapper, SetupDbContext);
        }

        #region GetAsync

        [Fact]
        public async Task GetAsyncReturnsACustomerWhenACustomerExists()
        {
            Customer setupCustomer = await CustomerTestDataFactory.CreateAndSaveAsync(SetupDbContext);
            Domain.Customers.Customer verifyCustomer = await _customerRepository.GetAsync(setupCustomer.Id);

            verifyCustomer.Should().NotBeNull();
            verifyCustomer.Name.Should().Be(setupCustomer.Name);
            verifyCustomer.Address.Should().Be(setupCustomer.Address);
            verifyCustomer.City.Should().Be(setupCustomer.City);
            verifyCustomer.State.Should().Be(setupCustomer.State);
            verifyCustomer.PostalCode.Should().Be(setupCustomer.PostalCode);
        }

        #endregion

        #region ListAsync

        [Fact]
        public async Task ListAsyncReturnsAListOfCustomersWhenCustomersExist()
        {
            var setupList = new List<Customer>
            {
                await CustomerTestDataFactory.CreateAndSaveAsync(SetupDbContext),
                await CustomerTestDataFactory.CreateAndSaveAsync(SetupDbContext)
            };

            IList<Domain.Customers.Customer> verifyCustomers = await _customerRepository.ListAsync();

            verifyCustomers.Should().NotBeNull();
            verifyCustomers.Count.Should().BeGreaterOrEqualTo(setupList.Count);
        }

        #endregion

        #region CreateAsync

        [Fact]
        public void CreateAsyncThrowsAnExceptionWhenCustomerIsNull()
        {
            FluentActions
                .Invoking(async () => await _customerRepository.CreateAsync(null, TestUser))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CreateAsyncThrowsAnExceptionWhenCreatedByIsNull()
        {
            FluentActions
                .Invoking(async () => await _customerRepository.CreateAsync(new Domain.Customers.Customer(), null))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task CreateAsyncReturnsACustomerWhenTheCustomerIsSaved()
        {
            var setupCustomer = new Domain.Customers.Customer
            {
                Name = Guid.NewGuid().ToString(),
                Address = Guid.NewGuid().ToString(),
                City = Guid.NewGuid().ToString(),
                State = Guid.NewGuid().ToString(),
                PostalCode = Guid.NewGuid().ToString()
            };

            var verifyCustomer = await _customerRepository.CreateAsync(setupCustomer, TestUser);

            verifyCustomer.Should().NotBeNull();
            verifyCustomer.Name.Should().Be(setupCustomer.Name);
            verifyCustomer.Address.Should().Be(setupCustomer.Address);
            verifyCustomer.City.Should().Be(setupCustomer.City);
            verifyCustomer.State.Should().Be(setupCustomer.State);
            verifyCustomer.PostalCode.Should().Be(setupCustomer.PostalCode);

        }

        #endregion

        #region UpdateAsync

        [Fact]
        public void UpdateAsyncThrowsAnExceptionWhenCustomerIsNull()
        {
            FluentActions
                .Invoking(async () => await _customerRepository.UpdateAsync(null, TestUser))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UpdateAsyncThrowsAnExceptionWhenCreatedByIsNull()
        {
            FluentActions
                .Invoking(async () => await _customerRepository.UpdateAsync(new Domain.Customers.Customer(), null))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UpdateAsyncThrowsAnExceptionWhenTheCustomerDoesNotExist()
        {
            FluentActions
                .Invoking(async () => await _customerRepository.UpdateAsync(new Domain.Customers.Customer(), TestUser))
                .Should().Throw<NullReferenceException>();
        }

        [Fact]
        public async Task UpdateAsyncReturnsACustomerWhenTheCustomerIsUpdated()
        {
            Customer setupCustomer = await CustomerTestDataFactory.CreateAndSaveAsync(SetupDbContext);
            Domain.Customers.Customer updateCustomer = new Domain.Customers.Customer
            {
                Id = setupCustomer.Id,
                Name = Guid.NewGuid().ToString(),
                Address = Guid.NewGuid().ToString(),
                City = Guid.NewGuid().ToString(),
                State = Guid.NewGuid().ToString(),
                PostalCode = Guid.NewGuid().ToString()
            };

            Domain.Customers.Customer verifyCustomer = await _customerRepository.UpdateAsync(updateCustomer, TestUser);

            verifyCustomer.Should().NotBeNull();
            verifyCustomer.Name.Should().Be(updateCustomer.Name);
            verifyCustomer.Address.Should().Be(updateCustomer.Address);
            verifyCustomer.City.Should().Be(updateCustomer.City);
            verifyCustomer.State.Should().Be(updateCustomer.State);
            verifyCustomer.PostalCode.Should().Be(updateCustomer.PostalCode);
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public void DeleteAsyncThrowsAnExceptionWhenCreatedByIsNull()
        {
            FluentActions
                .Invoking(async () => await _customerRepository.DeleteAsync(1, null))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteAsyncReturnsACompletedTaskWhenTheCustomerIsDeleted()
        {
            Customer setupCustomer = await CustomerTestDataFactory.CreateAndSaveAsync(SetupDbContext);
            Task verifyTask = _customerRepository.DeleteAsync(setupCustomer.Id, TestUser);
            verifyTask.Wait();

            verifyTask.Should().NotBeNull();
            verifyTask.Status.Should().Be(TaskStatus.RanToCompletion);
        }

        #endregion
    }
}