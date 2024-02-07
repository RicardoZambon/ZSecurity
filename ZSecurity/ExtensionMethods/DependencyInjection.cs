using Microsoft.Extensions.DependencyInjection;
using ZWebApi.Services;

namespace ZWebApi.ExtensionMethods
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSecurityHandlerService<TSecurityHandler>(this IServiceCollection services)
            where TSecurityHandler : class, ISecurityHandler
        {
            return services.AddScoped<ISecurityHandler, TSecurityHandler>()
                ;
        }
    }
}