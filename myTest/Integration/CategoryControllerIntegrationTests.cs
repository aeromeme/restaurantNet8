using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using restaurantAPI.DTO;
using Xunit;

namespace myTest.Integration
{
    public class CategoryControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CategoryControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllCategories_ReturnsOkAndList()
        {
            var response = await _client.GetAsync("/api/category");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
            Assert.NotNull(categories);
        }
    }
}