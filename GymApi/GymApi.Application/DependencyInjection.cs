using GymApi.Application.Interfaces;
using GymApi.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GymApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ITrainerService, TrainerService>();
        services.AddScoped<IWorkoutService, WorkoutService>();

        return services;
    }
}
