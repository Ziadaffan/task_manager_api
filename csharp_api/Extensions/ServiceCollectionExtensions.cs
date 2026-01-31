using Application.Interfaces;
using Application.Services;
using Infrastructure;
using Infrastructure.Interface;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace csharp_api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext (centralized). Adjust provider/connection string as needed.
            services.AddDbContext<TaskManagerContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            // Application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IOrganisationService, OrganisationService>();

            // Infrastructure / repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IOrganisationRepository, OrganisationRepository>();

            return services;
        }
    }
}