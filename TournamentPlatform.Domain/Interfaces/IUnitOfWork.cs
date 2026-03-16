namespace TournamentPlatform.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}