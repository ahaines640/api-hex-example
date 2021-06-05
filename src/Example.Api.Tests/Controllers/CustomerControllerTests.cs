using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Example.Api.Controllers;
using Example.Api.Models;
using Example.Domain.Customers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Example.Api.Tests.Controllers
{
    public class CustomerControllerTests : ApiTest
    {
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly CustomerController _customerController;

        public CustomerControllerTests()
        {
            _customerServiceMock = new Mock<ICustomerService>(MockBehavior.Strict);
            
            var loggerMock = new Mock<ILogger<CustomerController>>(MockBehavior.Strict);
            loggerMock.Setup(mock => mock.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

            _customerController = new CustomerController(
                Mapper,
                loggerMock.Object,
                _customerServiceMock.Object);
        }

        #region ListAll

        [Fact]
        public async Task ListAllReturnsAListOfCustomerModelsWhenCustomersExist()
        {
            var setupCustomers = new List<Customer>
            {
                new Customer(),
                new Customer()
            };

            _customerServiceMock.Reset();
            _customerServiceMock
                .Setup(mock => mock.ListAsync())
                .ReturnsAsync(setupCustomers);

            IActionResult result = await _customerController.ListAll();
            
            var resultObject = result as OkObjectResult;
            resultObject.Should().NotBeNull();
            resultObject?.StatusCode.Should().Be(200);

            var verifyCustomers = resultObject?.Value as List<CustomerModel>;
            verifyCustomers.Should().NotBeNull();
            verifyCustomers?.Count.Should().Be(setupCustomers.Count);
        }

        [Fact]
        public async Task ListAllReturnsAServerErrorWhenAnExceptionIsCaught()
        {
            _customerServiceMock.Reset();
            _customerServiceMock
                .Setup(mock => mock.ListAsync())
                .Throws<Exception>();

            IActionResult result = await _customerController.ListAll();
            
            StatusCodeResult statusCodeResult = result as StatusCodeResult;
            statusCodeResult.Should().NotBeNull();
            statusCodeResult?.StatusCode.Should().Be(500);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetByIdReturnsNotFoundWhenTheCustomerDoesNotExist()
        {
            int customerId = 2;

            _customerServiceMock.Reset();
            _customerServiceMock
                .Setup(mock => mock.GetAsync(customerId))
                .Returns(Task.FromResult((Customer) null));

            IActionResult result = await _customerController.GetById(customerId);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetByIdReturnsTheCustomerWhenTheCustomerExists()
        {
            int setupCustomerId = 2;

            _customerServiceMock.Reset();
            _customerServiceMock
                .Setup(mock => mock.GetAsync(setupCustomerId))
                .ReturnsAsync(new Customer {Id = setupCustomerId});

            IActionResult result = await _customerController.GetById(setupCustomerId);

            OkObjectResult okObjectResult = result as OkObjectResult;
            okObjectResult.Should().NotBeNull();

            CustomerModel verifyCustomer = okObjectResult?.Value as CustomerModel;
            verifyCustomer.Should().NotBeNull();
            verifyCustomer?.Id.Should().Be(setupCustomerId);
        }

        [Fact]
        public async Task GetByIdReturnsAServerErrorWhenAnExceptionIsCaught()
        {
            int setupCustomerId = 2;

            _customerServiceMock.Reset();
            _customerServiceMock
                .Setup(mock => mock.GetAsync(setupCustomerId))
                .Throws<Exception>();

            IActionResult result = await _customerController.GetById(setupCustomerId);

            StatusCodeResult statusCodeResult = result as StatusCodeResult;
            statusCodeResult.Should().NotBeNull();
            statusCodeResult?.StatusCode.Should().Be(500);
        }

        #endregion

        #region Create

        [Fact]
        public async Task CreateReturnsACustomerModelWhenTheCustomerIsCreated()
        {
            int setupCustomerId = 5;

            _customerServiceMock.Reset();
            _customerServiceMock
                .Setup(mock => mock.CreateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(new Customer {Id = setupCustomerId});

            IActionResult result = await _customerController.Create(new CustomerModel());

            CreatedAtActionResult createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            
            CustomerModel verifyCustomerModel = createdAtActionResult?.Value as CustomerModel;
            verifyCustomerModel.Should().NotBeNull();
            verifyCustomerModel?.Id.Should().Be(setupCustomerId);
        }

        [Fact]
        public async Task CreateReturnsAServerErrorWhenAnExceptionIsCaught()
        {
            _customerServiceMock.Reset();
            _customerServiceMock
                .Setup(mock => mock.CreateAsync(It.IsAny<Customer>()))
                .Throws<Exception>();

            IActionResult result = await _customerController.Create(new CustomerModel());

            StatusCodeResult statusCodeResult = result as StatusCodeResult;
            statusCodeResult.Should().NotBeNull();
            statusCodeResult?.StatusCode.Should().Be(500);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateReturnsNotFoundWhenTheCustomerDoesNotExist()
        {
            int setupCustomerId = 8;

            _customerServiceMock.Reset();
            _customerServiceMock
                .Setup(mock => mock.GetAsync(setupCustomerId))
                .Returns(Task.FromResult((Customer) null));

            IActionResult result = await _customerController.Update(setupCustomerId, new CustomerModel());
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateReturnsTheCustomerWhenTheCustomerIsUpdated()
        {
            int setupCustomerId = 6;

            _customerServiceMock.Reset();
            _customerServiceMock
                .Setup(mock => mock.GetAsync(setupCustomerId))
                .ReturnsAsync(new Customer {Id = setupCustomerId});
            _customerServiceMock
                .Setup(mock => mock.UpdateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(new Customer {Id = setupCustomerId});

            IActionResult result = await _customerController.Update(setupCustomerId, new CustomerModel());

            var okObjectResult = result as OkObjectResult;
            okObjectResult.Should().NotBeNull();

            var verifyCustomerModel = okObjectResult?.Value as CustomerModel;
            verifyCustomerModel.Should().NotBeNull();
            verifyCustomerModel?.Id.Should().Be(setupCustomerId);
        }

        [Fact]
        public async Task UpdateReturnsAServerErrorWhenAnExceptionIsCaught()
        {
            int setupCustomerId = 12;

            _customerServiceMock.Reset();
            _customerServiceMock
                .Setup(mock => mock.GetAsync(setupCustomerId))
                .Throws<Exception>();

            IActionResult result = await _customerController.Update(setupCustomerId, new CustomerModel());

            StatusCodeResult statusCodeResult = result as StatusCodeResult;
            statusCodeResult.Should().NotBeNull();
            statusCodeResult?.StatusCode.Should().Be(500);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteReturnsNotFoundWhenTheCustomerDoesNotExist()
        {
            int setupCustomerId = 4;

            _customerServiceMock.Reset();
            _customerServiceMock
                .Setup(mock => mock.GetAsync(setupCustomerId))
                .Returns(Task.FromResult((Customer) null));

            IActionResult result = await _customerController.Delete(setupCustomerId);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteReturnsNoContentWhenTheCustomerIsDeleted()
        {
            int setupCustomerId = 4;

            _customerServiceMock.Reset();
            _customerServiceMock
                .Setup(mock => mock.GetAsync(setupCustomerId))
                .ReturnsAsync(new Customer {Id = setupCustomerId});
            _customerServiceMock
                .Setup(mock => mock.DeleteAsync(setupCustomerId))
                .Returns(Task.CompletedTask);

            IActionResult result = await _customerController.Delete(setupCustomerId);
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteReturnsAServerErrorWhenAnExceptionIsCaught()
        {
            int setupCustomerId = 12;

            _customerServiceMock.Reset();
            _customerServiceMock
                .Setup(mock => mock.GetAsync(setupCustomerId))
                .Throws<Exception>();

            IActionResult result = await _customerController.Delete(setupCustomerId);

            StatusCodeResult statusCodeResult = result as StatusCodeResult;
            statusCodeResult.Should().NotBeNull();
            statusCodeResult?.StatusCode.Should().Be(500);
        }

        #endregion
    }
}
