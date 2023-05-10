using CoreModule.Base;
using CoreModule.Src;
using CoreModule.UnitOfWork;

namespace VillaApi
{
    public static class DIConfig
    {

        public static void UseVillaDIConfig(this IServiceCollection services)
        {
            services.AddTransient<UnitOfWorkServiceInterface, UnitOfWorkService>();
            services.AddScoped<VillaRepositoryInterface, VillaRepository>();
            services.AddScoped<VillaServiceInterface, VillaService>();
            services.AddScoped<VillaNumberRepositoryInterface, VillaNumberRepository>();
            services.AddScoped<VillaNumberServiceInterface, VillaNumberService>();
            services.AddScoped<UserServiceInterface,UserService>();
        }
    }
}
