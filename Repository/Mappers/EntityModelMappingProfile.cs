using AutoMapper;
using Repository.Models;
using Domain.Entities; // Assuming DTOs are here

namespace Repository.Mappers
{
    public class EntityModelMappingProfile : Profile
    {
        public EntityModelMappingProfile()
        {
            // Product: Model <-> Domain Entity
            CreateMap<ProductModel, Product>()
                .ReverseMap();

            // Category: Model <-> Domain Entity
            CreateMap<CategoryModel, Category>()
                .ReverseMap();


            CreateMap<Order, OrderModel>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailModel>().ReverseMap();

          

           

           
        }
    }
}