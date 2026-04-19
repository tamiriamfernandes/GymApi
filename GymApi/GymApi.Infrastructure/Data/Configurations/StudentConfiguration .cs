using GymApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymApi.Infrastructure.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("students");

        // PK
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.BirthDate)
            .IsRequired();

        // Enum como string (melhor pra leitura no banco 👀)
        builder.Property(x => x.Goal)
            .HasConversion<string>()
            .IsRequired();

        // Relacionamento opcional com User
        //builder.HasOne(x => x.User)
        //    .WithOne()
        //    .HasForeignKey<Student>(x => x.UserId)
        //    .OnDelete(DeleteBehavior.SetNull);

        // Índice útil
        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}
