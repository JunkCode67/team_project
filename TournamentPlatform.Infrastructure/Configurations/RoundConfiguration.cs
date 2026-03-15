using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentPlatform.Domain.Entities;

namespace TournamentPlatform.Infrastructure.Persistence.Configurations;

public class RoundConfiguration : IEntityTypeConfiguration<Round>
{
    public void Configure(EntityTypeBuilder<Round> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Title).IsRequired().HasMaxLength(200);
        builder.Property(r => r.Description).HasMaxLength(2000);
        builder.Property(r => r.Requirements).HasMaxLength(2000);
        builder.Property(r => r.Status).HasConversion<string>();

        builder.HasMany(r => r.Submissions)
            .WithOne(s => s.Round)
            .HasForeignKey(s => s.RoundId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}