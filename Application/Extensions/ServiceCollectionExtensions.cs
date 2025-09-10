using Application.UseCases.Products;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Extensions
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