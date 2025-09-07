using restaurantAPI.Application.Interfaces;
using restaurantAPI.Application.Services;
using Microsoft.EntityFrameworkCore;
using restaurantAPI.Application.Mappers;
using restaurantAPI.Models;
using restaurantAPI.Tools;
using restaurantAPI.UnitOfWork;
using restaurantAPI.Application.Products.UseCases;
using restaurantAPI.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<RestaurantContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:4200",    // Angular dev server
                "http://localhost:5173",
                "http://localhost:3000"// Vite, React, or another dev server
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.MapType<DateOnly>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Format = "date" // Swagger “date” format
    });
});
builder.Services.AddProductUseCases();
builder.Services.AddScoped<ProductAppService>();
builder.Services.AddScoped<IOrderAppService, OrderAppService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
