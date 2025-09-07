using AutoMapper;
using Moq;
using restaurantAPI.Application.Products.UseCases;
using restaurantAPI.Domain.Entities;
using restaurantAPI.DTO;
using restaurantAPI.Models;
using restaurantAPI.UnitOfWork;
using System.Threading.Tasks;
using Xunit;

namespace myTests.UseCase.Products {
    public class AddProductUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_ReturnsSuccess_WhenProductIsCreated()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            // Change the type of 'category' to restaurantAPI.Models.Category to match the expected return type
            var category = new restaurantAPI.Models.Category { CategoryId = 1, CategoryName = "Test" };
            var product = new ProductEntity { ProductName = "Prod", CategoryId = 1 };
            var ormProduct = new Product { ProductId = 123 };


            unitOfWorkMock.Setup(u => u.Categories.GetByIdAsync(1)).Returns(Task.FromResult<restaurantAPI.Models.Category?>(category));
            mapperMock.Setup(m => m.Map<ProductEntity>(It.IsAny<CreateProductDto>())).Returns(product);
            mapperMock.Setup(m => m.Map<Product>(product)).Returns(ormProduct);
            unitOfWorkMock.Setup(u => u.Products.AddAsync(ormProduct)).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.CompleteAsync()).Returns(Task.FromResult(1));

            var useCase = new AddProductUseCase(unitOfWorkMock.Object, mapperMock.Object);
            var dto = new CreateProductDto { Name = "Prod", CategoryId = 1 };

            // Act
            var (success, message, id) = await useCase.ExecuteAsync(dto);

            // Assert
            Assert.True(success);
            Assert.Equal("Product created successfully.", message);
            Assert.Equal(ormProduct.ProductId, id);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsFailure_WhenCategoryDoesNotExist()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            unitOfWorkMock.Setup(u => u.Categories.GetByIdAsync(99)).Returns(Task.FromResult<restaurantAPI.Models.Category?>(null));

            var useCase = new AddProductUseCase(unitOfWorkMock.Object, mapperMock.Object);
            var dto = new CreateProductDto { Name = "Prod", CategoryId = 99 };

            // Act
            var (success, message, id) = await useCase.ExecuteAsync(dto);

            // Assert
            Assert.False(success);
            Assert.Equal("Category does not exist.", message);
            Assert.Equal(0, id);
        }
    }
}
