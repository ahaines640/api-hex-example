using AutoMapper;
using Example.Data.Customers;
using Example.Data.Orders;
using Example.Domain;
using Example.Domain.Orders;
using Order = Example.Data.Orders.Order;

namespace Example.Data
{
    public class DataMappingProfile : Profile
    {
        public DataMappingProfile()
        {
            CreateMap<Entity, Model>();
            CreateMap<Model, Entity>()
                .ForMember(dest => dest.Modified, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<Customer, Domain.Customers.Customer>();
            CreateMap<Domain.Customers.Customer, Customer>()
                .IncludeBase<Model, Entity>()
                .ForMember(dest => dest.Orders, opt => opt.Ignore());

            CreateMap<Order, Domain.Orders.Order>();
            CreateMap<Domain.Orders.Order, Order>()
                .IncludeBase<Model, Entity>()
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

            CreateMap<OrderItem, Item>();
            CreateMap<Item, OrderItem>()
                .IncludeBase<Model, Entity>()
                .ForMember(dest => dest.Order, opt => opt.Ignore());
        }
    }
}