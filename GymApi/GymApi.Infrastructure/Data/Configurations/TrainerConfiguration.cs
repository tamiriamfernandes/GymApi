using GymApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymApi.Infrastructure.Data.Configurations;

public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.ToTable("trainers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.RegistrationNumber)
            .HasMaxLength(50);

        // relação opcional com User
        //builder.HasOne(x => x.User)
        //    .WithOne()
        //    .HasForeignKey<Trainer>(x => x.UserId)
        //    .OnDelete(DeleteBehavior.SetNull);
    }
}