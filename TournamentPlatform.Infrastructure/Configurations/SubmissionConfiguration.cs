using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentPlatform.Domain.Entities;

namespace TournamentPlatform.Infrastructure.Persistence.Configurations;

public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
{
    public void Configure(EntityTypeBuilder<Submission> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.GitHubUrl).IsRequired().HasMaxLength(500);
        builder.Property(s => s.VideoUrl).IsRequired().HasMaxLength(500);
        builder.Property(s => s.LiveDemoUrl).HasMaxLength(500);
        builder.Property(s => s.Description).HasMaxLength(2000);
        builder.Property(s => s.Status).HasConversion<string>();

        builder.HasMany(s => s.Evaluations)
            .WithOne(e => e.Submission)
            .HasForeignKey(e => e.SubmissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}