using ZDatabase.Services.Interfaces;
using ZSecurity.Repositories.Interfaces;
using ZSecurity.Services;
using ZSecurity.UnitTests.Fakes.Entities;

namespace ZSecurity.UnitTests.Fakes.Services
{
    /// <summary>
    /// A handler that replaces the default administrators action code, so the protected
    /// <c>AdministratorsActionCode</c> extension point can be asserted.
    /// </summary>
    internal sealed class SecurityHandlerFake : SecurityHandler<IBaseUsersRepository<ActionEntityFake, long>, ActionEntityFake, long>
    {
        internal const string CustomAdministratorsActionCode = "CustomAdministrators";

        public SecurityHandlerFake(
                IBaseUsersRepository<ActionEntityFake, long> baseUsersRepository,
                ICurrentUserProvider<long> currentUserProvider)
            : base(baseUsersRepository, currentUserProvider)
        {
        }

        protected override string AdministratorsActionCode
        {
            get { return CustomAdministratorsActionCode; }
        }
    }
}
