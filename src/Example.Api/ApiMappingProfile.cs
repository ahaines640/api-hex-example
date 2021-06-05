using AutoMapper;

namespace Example.Api
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<Domain.Customers.Customer, Models.CustomerModel>();
            CreateMap<Models.CustomerModel, Domain.Customers.Customer>();
        }
    }
}