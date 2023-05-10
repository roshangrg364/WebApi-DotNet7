using Villa_MVC_Core_Module.Base;
using Villa_MVC_Core_Module.Service;


namespace Villa_Web_App
{
    public static class DiConfig
    {

        public static void UseVilla(this IServiceCollection services)
        {
            services.AddHttpClient<BaseServiceInterface, BaseService>();
            services.AddScoped<VillaServiceInterface, VillaService>();
            services.AddScoped<VillaNumberServiceInterface, VillaNumberService>();
            services.AddScoped<AccountServiceInterface, AccountService>();
        }
    }
}
