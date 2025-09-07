using Microsoft.Extensions.DependencyInjection;
using restaurantAPI.Application.Products.UseCases;

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
    }
}