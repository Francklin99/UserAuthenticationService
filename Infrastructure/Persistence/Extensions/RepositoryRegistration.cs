using Domain.Common;
using Domain.Repositories;
using Infrastructure.Persistence.common;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Extensions
{
    public static class RepositoryRegistration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
