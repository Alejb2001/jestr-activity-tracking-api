using ActivityTracker.Application.Interfaces;
using ActivityTracker.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityTracker.Application.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IActivityService, ActivityService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICompanyService, CompanyService>();
        return services;
    }
}
