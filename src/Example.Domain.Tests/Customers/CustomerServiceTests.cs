using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Example.Domain.Customers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Example.Domain.Tests.Customers
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly CustomerService _customerService;
        private readonly Mock<IUserContextResolver> _userContextResolverMock;

        public CustomerServiceTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>(MockBehavior.Strict);
            _userContextResolverMock = new Mock<IUserContextResolver>(MockBehavior.Strict);

            _customerService = new CustomerService(
                _userContextResolverMock.Object,
                _customerRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAsyncReturnsACustomerWhenTheCustomerExists()
        {
            int customerId = Guid.NewGuid().GetHashCode();

            _customerRepositoryMock.Reset();
            _customerRepositoryMock
                .Setup(mock => mock.GetAsync(customerId))
                .ReturnsAsync(new Customer {Id = customerId});

            Customer result = await _customerService.GetAsync(customerId);

            result.Should().NotBeNull();
            result.Id.Should().Be(customerId);

            _customerRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task ListAsyncReturnsAListOfCustomersWhenCustomersExist()
        {
            _customerRepositoryMock.Reset();
            _customerRepositoryMock
                .Setup(mock => mock.ListAsync())
                .ReturnsAsync(new List<Customer>());

            IList<Customer> result = await _customerService.ListAsync();
            
            result.Should().NotBeNull();
            _customerRepositoryMock.VerifyAll();
        }

        [Fact]
        public void CreateAsyncThrowsAnExceptionWhenCustomerIsNull()
        {
            FluentActions
                .Invoking(async () => await _customerService.CreateAsync(null))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task CreateAsyncReturnsACustomerWhenTheCustomerIsCreated()
        {
            var setupCustomer = new Customer { Id = Guid.NewGuid().GetHashCode()};

            _userContextResolverMock.Reset();
            _userContextResolverMock
                .Setup(mock => mock.CurrentUser)
                .Returns(Guid.NewGuid().ToString());

            _customerRepositoryMock.Reset();
            _customerRepositoryMock
                .Setup(mock => mock.CreateAsync(setupCustomer, It.IsAny<string>()))
                .ReturnsAsync(setupCustomer);

            Customer verifyCustomer = await _customerService.CreateAsync(setupCustomer);

            verifyCustomer.Should().NotBeNull();
            verifyCustomer.Id.Should().Be(setupCustomer.Id);

            _userContextResolverMock.VerifyAll();
            _customerRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task UpdateAsyncReturnsACustomerWhenTheCustomerIsUpdated()
        {
            var setupCustomer = new Customer { Id = Guid.NewGuid().GetHashCode() };

            _userContextResolverMock.Reset();
            _userContextResolverMock
                .Setup(mock => mock.CurrentUser)
                .Returns(Guid.NewGuid().ToString());

            _customerRepositoryMock.Reset();
            _customerRepositoryMock
                .Setup(mock => mock.UpdateAsync(setupCustomer, It.IsAny<string>()))
                .ReturnsAsync(setupCustomer);

            Customer verifyCustomer = await _customerService.UpdateAsync(setupCustomer);

            verifyCustomer.Should().NotBeNull();
            verifyCustomer.Id.Should().Be(setupCustomer.Id);

            _userContextResolverMock.VerifyAll();
            _customerRepositoryMock.VerifyAll();
        }

        [Fact]
        public void DeleteAsyncRetunsACompletedTaskWhenTheCustomerIsDeleted()
        {
            int setupCustomerId = Guid.NewGuid().GetHashCode();

            _userContextResolverMock.Reset();
            _userContextResolverMock
                .Setup(mock => mock.CurrentUser)
                .Returns(Guid.NewGuid().ToString());

            _customerRepositoryMock.Reset();
            _customerRepositoryMock
                .Setup(mock => mock.DeleteAsync(setupCustomerId, It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            Task verifyTask = _customerService.DeleteAsync(setupCustomerId);
            verifyTask.Wait();

            verifyTask.Should().NotBeNull();
            verifyTask.Status.Should().Be(TaskStatus.RanToCompletion);
        }
    }
}