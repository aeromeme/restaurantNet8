using Microsoft.Extensions.DependencyInjection;
using restaurantAPI.Application.Products.UseCases;
using restaurantAPI.Models;
using restaurantAPI.Repostiories;

namespace restaurantAPI.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProductUseCases(this IServiceCollection services)
        {
            services.AddScoped<AddProductUseCase>();
            services.AddScoped<GetAllProductsUseCase>();
            services.AddScoped<GetProductByIdUseCase>();
            services.AddScoped<UpdateProductUseCase>();
            services.AddScoped<DeleteProductUseCase>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRepository<Category>,Repository<Category>>();
            services.AddScoped<IRepository<Order>,Repository<Order>>();
            services.AddScoped<IRepository<OrderDetail>,Repository<OrderDetail>>();
            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IRepository<Fan>, Repository<Fan>>();

            return services;
        }
    }
}