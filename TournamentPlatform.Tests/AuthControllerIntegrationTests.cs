using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TournamentPlatform.Application.DTO.Auth;
using TournamentPlatform.Infrastructure.Persistence;

namespace TournamentPlatform.Tests
{
    public class AuthControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            var testFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // --- НОВИЙ ГЛОБАЛЬНИЙ БЛОК ОЧИСТКИ ---
                    // Знаходимо і видаляємо всі налаштування DbContext
                    var dbContextDescriptors = services.Where(
                        d => d.ServiceType.Name.Contains("DbContextOptions")).ToList();

                    foreach (var descriptor in dbContextDescriptors)
                    {
                        services.Remove(descriptor);
                    }

                    // Видаляємо підключення DbConnection, якщо воно є
                    var dbConnectionDescriptor = services.SingleOrDefault(
                        d => d.ServiceType.Name.Contains("DbConnection"));

                    if (dbConnectionDescriptor != null)
                    {
                        services.Remove(dbConnectionDescriptor);
                    }
                    // --------------------------------------

                    // Тепер безпечно підключаємо In-Memory базу
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase("TestAuthDb"));
                });
            });

            _client = testFactory.CreateClient();
        }

        [Fact]
        public async Task Register_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = "test_max@example.com",
                Name = "MaxTest", 
                Password = "SuperSecretPassword123!"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Auth/register", registerDto);
            var responseText = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"бо API повернув помилку: {responseText}");
        }
        
        
        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnSuccessAndToken()
        {
            // Arrange (Готуємо дані)
            var email = "login_tester@example.com";
            var password = "SuperSecretPassword123!";
    
            // 1. Спочатку створюємо користувача в нашій тестовій базі
            var registerDto = new RegisterDto
            {
                Email = email,
                Name = "LoginTester",
                Password = password
            };
            await _client.PostAsJsonAsync("/api/Auth/register", registerDto);

            // 2. Формуємо дані безпосередньо для входу
            var loginDto = new LoginDto // Переконайся, що DTO називається саме так
            {
                Email = email,
                Password = password
            };

            // Act (Відправляємо POST запит на логін)
            var response = await _client.PostAsJsonAsync("/api/Auth/login", loginDto);
            var responseText = await response.Content.ReadAsStringAsync();

            // Assert (Перевіряємо результат)
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"Помилка логіну: {responseText}");
    
            // Перевіряємо, що сервер дійсно видав нам JWT токен у відповіді
            responseText.ToLower().Should().Contain("token"); 
        }
    }
}