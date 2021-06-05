using AutoMapper;
using Xunit;

namespace Example.Api.Tests
{
    public class ApiMappingProfileTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            new MapperConfiguration(cfg => cfg.AddProfile(typeof(ApiMappingProfile))).AssertConfigurationIsValid();
        }
    }
}