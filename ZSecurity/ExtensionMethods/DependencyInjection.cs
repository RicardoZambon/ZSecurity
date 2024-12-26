using Microsoft.Extensions.DependencyInjection;
using ZSecurity.Services;

namespace ZSecurity.ExtensionMethods
{
    /// <summary>
    /// Extension methods for <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds the security handler service.
        /// </summary>
        /// <typeparam name="TSecurityHandler">The type of the security handler.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddSecurityHandlerService<TSecurityHandler>(this IServiceCollection services)
            where TSecurityHandler : class, ISecurityHandler
        {
            return services.AddScoped<ISecurityHandler, TSecurityHandler>();
        }
    }
}