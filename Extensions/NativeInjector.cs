using Portfolium_Back.Connections.Repositories;
using Portfolium_Back.Connections.Repositories.Interface;
using Portfolium_Back.Services;
using Portfolium_Back.Services.Interfaces;

namespace Portfolium_Back.Extensions
{
    public class NativeInjector
    {
        public static void RegisterServices(IServiceCollection services)
        {
            #region Services

            services.AddScoped<IUserService, UserService>();

            #endregion Services

            #region Repositories

            services.AddScoped<IUserRepository, UserRepository>();

            #endregion Repositories
        }
    }
}