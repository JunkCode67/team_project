using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentPlatform.Domain.Entities;

namespace TournamentPlatform.Infrastructure.Persistence.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Organization).HasMaxLength(200);

        builder.HasOne(t => t.Captain)
            .WithOne()
            .HasForeignKey<Team>(t => t.CaptainId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.Members)
            .WithOne(m => m.Team)
            .HasForeignKey(m => m.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Submissions)
            .WithOne(s => s.Team)
            .HasForeignKey(s => s.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}