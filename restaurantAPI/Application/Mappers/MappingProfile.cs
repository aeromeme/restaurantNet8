using AutoMapper;
using restaurantAPI.DTO;
using restaurantAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace restaurantAPI.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.StockQuantity))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Order, OrderDto>();
            CreateMap<OrderDetail, OrderDetailDto>();

            CreateMap<OrderDetailDto, OrderDetail>()
                .ForMember(dest => dest.Order, opt => opt.Ignore())
               .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<CreateOrderDto, Order>();
            CreateMap<CreateOrderDetailDto, OrderDetail>();
        }
    }
}
