using AutoMapper;
using Domain.Entities;
using Application.DTO;

namespace Application.Mappers
{
    public class EntityModelMappingProfile : Profile
    {
        public EntityModelMappingProfile()
        {
            //// Product: Model <-> Domain Entity
            //CreateMap<Models.Product, Product>()
            //    .ReverseMap();

            //// Category: Model <-> Domain Entity
            //CreateMap<Models.Category, Category>()
            //    .ReverseMap();



            // Domain Entity -> DTO
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.StockQuantity))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<CreateProductDto, Product>()
               .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<UpdateProductDto, Domain.Entities.Product>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Category, opt => opt.Ignore());


            CreateMap<Domain.Entities.Order, OrderDto>();
            CreateMap<Domain.Entities.OrderDetail, OrderDetailDto>();

            CreateMap<OrderDetailDto, Domain.Entities.OrderDetail>()
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            //// Order: Model <-> Domain Entity
            //CreateMap<Models.Order, Domain.Entities.Order>().ReverseMap();

            //CreateMap<Models.OrderDetail, Domain.Entities.OrderDetail>().ReverseMap();

           

            CreateMap<CreateOrderDto, Domain.Entities.Order>();
            CreateMap<CreateOrderDetailDto, Domain.Entities.OrderDetail>();
        }
    }
}