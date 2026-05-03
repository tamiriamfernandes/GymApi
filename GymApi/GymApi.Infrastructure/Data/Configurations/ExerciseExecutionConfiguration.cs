using GymApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymApi.Infrastructure.Data.Configurations;

public class ExerciseExecutionConfiguration : IEntityTypeConfiguration<ExerciseExecution>
{
    public void Configure(EntityTypeBuilder<ExerciseExecution> builder)
    {
        builder.ToTable("exercise_executions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.WorkoutExecutionId)
            .IsRequired();

        builder.Property(x => x.ExerciseId)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.CompletedAt);

        builder.Property(x => x.PerformedWeight)
            .HasColumnType("numeric");

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.HasOne(x => x.WorkoutExecution)
            .WithMany(x => x.ExerciseExecutions)
            .HasForeignKey(x => x.WorkoutExecutionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Exercise)
            .WithMany(x => x.ExerciseExecutions)
            .HasForeignKey(x => x.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
