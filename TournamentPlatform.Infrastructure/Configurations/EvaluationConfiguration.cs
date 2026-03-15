using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentPlatform.Domain.Entities;
namespace TournamentPlatform.Infrastructure.Configurations;

public class EvaluationConfiguration : IEntityTypeConfiguration<Evaluation>
{
    public void Configure(EntityTypeBuilder<Evaluation> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property<object>(e => e.Comment).HasMaxLength(1000);

        builder.HasOne(e => e.Jury)
            .WithMany()
            .HasForeignKey(e => e.JuryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.SubmissionId, e.JuryId }).IsUnique();
    }
}