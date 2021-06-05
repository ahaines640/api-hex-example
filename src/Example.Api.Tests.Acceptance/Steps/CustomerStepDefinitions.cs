using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Example.Api.Tests.Acceptance.ApiModels;
using Example.Api.Tests.Acceptance.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace Example.Api.Tests.Acceptance.Steps
{
    [Binding]
    public class CustomerStepDefinitions
    {
        private const string SetupCustomerContextKey = "Example.Api.Tests.Acceptance.Steps.CustomerStepDefinitions.SetupCustomer";
        private const string HttpResponseMessageContextKey = "Example.Api.Tests.Acceptance.Steps.CustomerStepDefinitions.HttpResponseMessage";

        private readonly ScenarioContext _scenarioContext;
        private readonly ExampleDbContext _dbContext;

        public CustomerStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            _dbContext = new ExampleDbContext(new DbContextOptionsBuilder<ExampleDbContext>()
                .UseSqlServer(Settings.DbConnection)
                .Options);
        }

        [Given(@"a customer exists")]
        public async Task GivenACustomerExists()
        {
            var setupCustomer = new Customer
            {
                Name = Guid.NewGuid().ToString(),
                Address = Guid.NewGuid().ToString(),
                City = Guid.NewGuid().ToString(),
                State = Guid.NewGuid().ToString(),
                PostalCode = Guid.NewGuid().ToString().Substring(0, 25),
            };

            await _dbContext.Customers.AddAsync(setupCustomer);
            await _dbContext.SaveChangesAsync();
            _scenarioContext.Add(SetupCustomerContextKey, setupCustomer);
        }

        [When(@"the customer is asked for")]
        public async Task WhenTheCustomerIsAskedFor()
        {
            var setupCustomer = _scenarioContext.Get<Customer>(SetupCustomerContextKey);
            var requestUri = $"{Settings.ApiBaseUrl}/customers/{setupCustomer.Id}";

            var client = new HttpClient();
            HttpResponseMessage responseMessage =
                await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri));

            _scenarioContext.Add(HttpResponseMessageContextKey, responseMessage);
        }

        [Then(@"the result is Ok")]
        public void ThenTheResultIsOk()
        {
            var responseMessage = _scenarioContext.Get<HttpResponseMessage>(HttpResponseMessageContextKey);
            
            responseMessage.Should().NotBeNull();
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then(@"the result contains the customer")]
        public async Task ThenTheResultContainsTheCustomer()
        {
            var responseMessage = _scenarioContext.Get<HttpResponseMessage>(HttpResponseMessageContextKey);
            var setupCustomer = _scenarioContext.Get<Customer>(SetupCustomerContextKey);

            var validateCustomer =
                JsonConvert.DeserializeObject<CustomerModel>(await responseMessage.Content.ReadAsStringAsync());

            validateCustomer.Should().NotBeNull();
            validateCustomer?.Name.Should().Be(setupCustomer.Name);
            validateCustomer?.Address.Should().Be(setupCustomer.Address);
            validateCustomer?.City.Should().Be(setupCustomer.City);
            validateCustomer?.State.Should().Be(setupCustomer.State);
            validateCustomer?.PostalCode.Should().Be(setupCustomer.PostalCode);
        }
    }
}