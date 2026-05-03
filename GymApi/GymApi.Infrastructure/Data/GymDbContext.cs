using GymApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymApi.Infrastructure.Data;

public class GymDbContext : DbContext
{
    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
    {
        
    }

    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutExecution> WorkoutExecutions => Set<WorkoutExecution>();
    public DbSet<ExerciseExecution> ExerciseExecutions => Set<ExerciseExecution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>();
        modelBuilder.Entity<Trainer>();
        modelBuilder.Entity<Workout>();
        modelBuilder.Entity<Exercise>();
        modelBuilder.Entity<WorkoutExecution>();
        modelBuilder.Entity<ExerciseExecution>();
    }
}
