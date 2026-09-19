using System.Runtime.CompilerServices;
using ZSecurity.Attributes;
using ZSecurity.Services;

namespace ZSecurity.UnitTests.Fakes.Services
{
    /// <summary>
    /// A guarded service that implements no interface, so the action name the handler builds has
    /// an empty service part.
    /// </summary>
    internal sealed class InterfacelessServiceFake
    {
        private readonly ISecurityHandler securityHandler;

        internal InterfacelessServiceFake(ISecurityHandler securityHandler)
        {
            this.securityHandler = securityHandler;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [ActionMethod]
        public async Task DefaultActionAsync()
        {
            await securityHandler.ValidateUserHasPermissionAsync();
        }
    }
}
