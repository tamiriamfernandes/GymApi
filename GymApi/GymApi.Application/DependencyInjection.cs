using FluentValidation;
using GymApi.Application.Interfaces;
using GymApi.Application.Services;
using GymApi.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace GymApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateStudentDtoValidator>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ITrainerService, TrainerService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<IExerciseService, ExerciseService>();
        services.AddScoped<IWorkoutExecutionService, WorkoutExecutionService>();

        return services;
    }
}
