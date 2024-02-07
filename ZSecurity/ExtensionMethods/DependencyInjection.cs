using Microsoft.Extensions.DependencyInjection;
using ZSecurity.Services;

namespace ZSecurity.ExtensionMethods
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