using AutoMapper;
using Xunit;

namespace Example.Data.Tests
{
    public class DataMappingProfileTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            new MapperConfiguration(cfg => cfg.AddProfile(typeof(DataMappingProfile)))
                .AssertConfigurationIsValid();
        }
    }
}