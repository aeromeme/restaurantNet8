using AutoMapper;
using restaurantAPI.Domain.Entities;
using restaurantAPI.Models;
using restaurantAPI.DTO; // Assuming DTOs are here

namespace restaurantAPI.Application.Mappers
{
    public class EntityModelMappingProfile : Profile
    {
        public EntityModelMappingProfile()
        {
            // Product: Model <-> Domain Entity
            CreateMap<Models.Product, Domain.Entities.Product>()
                .ReverseMap();

            // Category: Model <-> Domain Entity
            CreateMap<Models.Category, Domain.Entities.Category>()
                .ReverseMap();



            // Domain Entity -> DTO
            CreateMap<Domain.Entities.Product, ProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.StockQuantity))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            CreateMap<Domain.Entities.Category, CategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<CreateProductDto, Domain.Entities.Product>()
               .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<UpdateProductDto, Domain.Entities.Product>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Category, opt => opt.Ignore());


            CreateMap<Domain.Entities.Order, OrderDto>();
            CreateMap<Domain.Entities.OrderDetail, OrderDetailDto>();

            // Order: Model <-> Domain Entity
            CreateMap<Models.Order, Domain.Entities.Order>();

            CreateMap<Models.OrderDetail, Domain.Entities.OrderDetail>();

            CreateMap<OrderDetailDto, Domain.Entities.OrderDetail>();

            CreateMap<CreateOrderDto, Domain.Entities.Order>();
            CreateMap<CreateOrderDetailDto, Domain.Entities.OrderDetail>();
        }
    }
}