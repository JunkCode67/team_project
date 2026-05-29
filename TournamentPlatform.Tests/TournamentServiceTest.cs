using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using TournamentPlatform.Domain.Entities;
using TournamentPlatform.Domain.Enums;
using TournamentPlatform.Infrastructure.Persistence;
using TournamentPlatform.Infrastructure.Services;
// Додай using для твого AppDbContext

namespace TournamentPlatform.Tests.Services
{
    public class TournamentServiceTests
    {
        private async Task<AppDbContext> GetDbContextAsync(string dbName)
        {
            // Створюємо "віртуальну" базу даних у пам'яті
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenTournamentExists_ShouldUpdateStatus()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString(); // Унікальне ім'я бази для кожного тесту
            using var context = await GetDbContextAsync(dbName);
            
            // Створюємо реальний UnitOfWork та Сервіс!
            var uow = new UnitOfWork(context);
            var service = new TournamentService(uow);

            var tournamentId = Guid.NewGuid();
            var tournament = new Tournament 
            { 
                Id = tournamentId, 
                Title = "Test Tournament", 
                Status = TournamentStatus.Draft // Твій реальний статус
            };

            // Додаємо тестовий турнір у нашу In-Memory базу
            context.Tournaments.Add(tournament);
            await context.SaveChangesAsync();

            // Act
            // Викликаємо метод сервісу (наприклад, змінюємо на статус InProgress або який там у тебе є)
            var result = await service.UpdateStatusAsync(tournamentId, TournamentStatus.Finished); // Заміни на свій статус

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(TournamentStatus.Finished);

            // Перевіряємо, чи дійсно статус зберігся у базі
            var updatedTournament = await context.Tournaments.FindAsync(tournamentId);
            updatedTournament.Status.Should().Be(TournamentStatus.Finished);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenTournamentDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var context = await GetDbContextAsync(dbName);
            
            var uow = new UnitOfWork(context);
            var service = new TournamentService(uow);

            // Act
            Func<Task> act = async () => await service.UpdateStatusAsync(Guid.NewGuid(), TournamentStatus.Finished);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Турнір не знайдено");
        }
    }
}