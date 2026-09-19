using System.Runtime.CompilerServices;
using ZSecurity.Attributes;
using ZSecurity.Enums;
using ZSecurity.Services;

namespace ZSecurity.UnitTests.Fakes.Services
{
    /// <summary>
    /// A service whose methods are guarded by <see cref="ActionMethodAttribute"/>, standing in for
    /// the real consumer of <see cref="ISecurityHandler.ValidateUserHasPermissionAsync"/>.
    /// </summary>
    internal sealed class SecuredServiceFake : ISecuredServiceFake
    {
        private readonly ISecurityHandler securityHandler;

        internal SecuredServiceFake(ISecurityHandler securityHandler)
        {
            this.securityHandler = securityHandler;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [ActionMethod]
        public async Task DefaultActionAsync()
        {
            await securityHandler.ValidateUserHasPermissionAsync();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [ActionMethod(ActionTypes.OnlyAdmins)]
        public async Task OnlyAdminsActionAsync()
        {
            await securityHandler.ValidateUserHasPermissionAsync();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [ActionMethod(ActionTypes.OnlyRegularUsers)]
        public async Task OnlyRegularUsersActionAsync()
        {
            await securityHandler.ValidateUserHasPermissionAsync();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public async Task UnattributedActionAsync()
        {
            await securityHandler.ValidateUserHasPermissionAsync();
        }

        /// <summary>
        /// An unattributed entry point that calls through an attributed one, so the helper has to
        /// walk past the first frame before it finds the attribute.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public async Task NestedCallActionAsync()
        {
            await DefaultActionAsync();
        }
    }
}
