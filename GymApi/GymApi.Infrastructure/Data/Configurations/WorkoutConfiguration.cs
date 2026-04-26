using GymApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymApi.Infrastructure.Data.Configurations;

public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.ToTable("workout");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
           .IsRequired()
           .HasMaxLength(150);

        builder.Property(x => x.Description)
           .IsRequired()
           .HasMaxLength(500);

        builder.Property(x => x.StudentId)
           .IsRequired();

        builder.Property(x => x.TrainerId)
           .IsRequired();

        builder.Property(x => x.IsActive)
           .IsRequired();

        builder.Property(x => x.CreatedAt)
           .IsRequired();

        builder.Property(x => x.UpdatedAt);
    }
}
