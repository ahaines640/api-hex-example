using AutoMapper;

namespace Example.Api.Tests
{
    public class ApiTest
    {
        protected IMapper Mapper;

        protected ApiTest()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile(typeof(ApiMappingProfile))).CreateMapper();
        }
    }
}