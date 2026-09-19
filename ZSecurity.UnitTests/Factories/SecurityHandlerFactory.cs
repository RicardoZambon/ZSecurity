using ZDatabase.Services.Interfaces;
using ZSecurity.Repositories.Interfaces;
using ZSecurity.Services;
using ZSecurity.UnitTests.Fakes.Entities;

namespace ZSecurity.UnitTests.Factories
{
    internal static class SecurityHandlerFactory
    {
        /// <summary>
        /// The action code the library uses for administrators when it is not overridden.
        /// </summary>
        internal const string DefaultAdministratorsActionCode = "AdministrativeMaster";

        /// <summary>
        /// The user identifier used by every handler built here.
        /// </summary>
        internal const long UserID = 42;

        internal static IBaseUsersRepository<ActionEntityFake, long> CreateRepository(bool hasAnyAction)
        {
            IBaseUsersRepository<ActionEntityFake, long> repository = Substitute.For<IBaseUsersRepository<ActionEntityFake, long>>();
            repository.HasAnyActionAsync(Arg.Any<long>(), Arg.Any<string[]>()).Returns(hasAnyAction);
            return repository;
        }

        internal static ICurrentUserProvider<long> CreateCurrentUserProvider(long? currentUserID)
        {
            ICurrentUserProvider<long> currentUserProvider = Substitute.For<ICurrentUserProvider<long>>();
            currentUserProvider.CurrentUserID.Returns(currentUserID);
            return currentUserProvider;
        }

        internal static SecurityHandler<IBaseUsersRepository<ActionEntityFake, long>, ActionEntityFake, long> Create(
            IBaseUsersRepository<ActionEntityFake, long> repository,
            ICurrentUserProvider<long> currentUserProvider)
        {
            return new SecurityHandler<IBaseUsersRepository<ActionEntityFake, long>, ActionEntityFake, long>(repository, currentUserProvider);
        }
    }
}
