using GymApi.Application.Interfaces;
using GymApi.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();

        services.AddScoped<ITrainerService, TrainerService>();

        return services;
    }
}
