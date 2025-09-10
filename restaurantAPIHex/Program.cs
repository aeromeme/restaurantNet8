using Application.Interfaces;
using Application.UseCases.ProductsGenerics;
using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository.UnitOfWork;
using Repository.Repositories;
using Application.Services;
using Application.UseCases.OrdersGenerics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//add persistence
builder.Services.AddDbContext<RestaurantContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//repositories
builder.Services.AddScoped<IRepository<ProductModel>,Repository<ProductModel>>();
builder.Services.AddScoped<IRepository<CategoryModel>, Repository<CategoryModel>>();
builder.Services.AddScoped<IDetailGenericRepository<ProductModel>, ProductGenericRepository>();
builder.Services.AddScoped<IDetailGenericRepository<OrderModel>, OrderGenericRepository>();

//unit of work
builder.Services.AddScoped<IUnitOfWorkGeneric, UnitOfWorkGeneric>();

//automapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(Repository.Mappers.EntityModelMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(Application.Mappers.EntityModelMappingProfile).Assembly);

//use cases
builder.Services.AddScoped<IGetAllProductsUseCase<ProductModel>, GetAllProductsUseCase<ProductModel>>();
builder.Services.AddScoped<IGetProductByIdUseCase<ProductModel>, GetProductByIdUseCase<ProductModel>>();
builder.Services.AddScoped<IAddProductUseCase<CategoryModel, ProductModel>, AddProductUseCase<CategoryModel,ProductModel>>();
builder.Services.AddScoped<IUpdateProductUseCase<CategoryModel,ProductModel>, UpdateProductUseCase<CategoryModel,ProductModel>>();
builder.Services.AddScoped<IDeleteProductUseCase<ProductModel>, DeleteProductUseCase<ProductModel>>();
builder.Services.AddScoped<ICategoryAppService<CategoryModel>, CategoryAppService<CategoryModel>>();
builder.Services.AddScoped<IGetAllOrderUseCase<OrderModel>,GetAllOrderUseCase<OrderModel>>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
