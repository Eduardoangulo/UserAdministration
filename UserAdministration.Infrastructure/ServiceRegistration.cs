using Microsoft.Extensions.DependencyInjection;
using UserAdministration.Application;
using UserAdministration.Application.Interfaces;
using UserAdministration.Infrastructure.Repository;

namespace UserAdministration.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserUseCase, UserUseCase>();
        }
    }
}
