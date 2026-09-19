using ZSecurity.Exceptions;
using ZSecurity.Repositories.Interfaces;
using ZSecurity.Services;
using ZSecurity.UnitTests.Factories;
using ZSecurity.UnitTests.Fakes.Entities;
using ZSecurity.UnitTests.Fakes.Services;

namespace ZSecurity.UnitTests.Services
{
    /// <summary>
    /// Unit tests for <see cref="ZSecurity.Services.SecurityHandler{TBaseUserRepository, TActions, TUsersKey}"/>.
    /// </summary>
    public class SecurityHandlerTests
    {
        private const string AdministratorsActionCode = SecurityHandlerFactory.DefaultAdministratorsActionCode;
        private const long UserID = SecurityHandlerFactory.UserID;

        #region CheckCurrentUserIsAdministratorAsync
        /// <summary>
        /// Test the CheckCurrentUserIsAdministratorAsync should return true when the repository confirms the administrators action.
        /// </summary>
        [Fact]
        public async Task CheckCurrentUserIsAdministratorAsync_Pass_ReturnsTrueWhenTheUserHasTheAdministratorsAction()
        {
            // Arrange
            ISecurityHandler handler = SecurityHandlerFactory.Create(
                SecurityHandlerFactory.CreateRepository(true),
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID));

            // Act
            bool result = await handler.CheckCurrentUserIsAdministratorAsync();

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Test the CheckCurrentUserIsAdministratorAsync should return false when the repository denies the administrators action.
        /// </summary>
        [Fact]
        public async Task CheckCurrentUserIsAdministratorAsync_Pass_ReturnsFalseWhenTheUserDoesNotHaveTheAdministratorsAction()
        {
            // Arrange
            ISecurityHandler handler = SecurityHandlerFactory.Create(
                SecurityHandlerFactory.CreateRepository(false),
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID));

            // Act
            bool result = await handler.CheckCurrentUserIsAdministratorAsync();

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Test the CheckCurrentUserIsAdministratorAsync should return false when there is no current user.
        /// </summary>
        [Fact]
        public async Task CheckCurrentUserIsAdministratorAsync_Fail_ReturnsFalseWhenThereIsNoCurrentUser()
        {
            // Arrange
            IBaseUsersRepository<ActionEntityFake, long> repository = SecurityHandlerFactory.CreateRepository(true);

            ISecurityHandler handler = SecurityHandlerFactory.Create(
                repository,
                SecurityHandlerFactory.CreateCurrentUserProvider(null));

            // Act
            bool result = await handler.CheckCurrentUserIsAdministratorAsync();

            // Assert
            result.Should().BeFalse();

            await repository.DidNotReceiveWithAnyArgs().HasAnyActionAsync(default, default!);
        }

        /// <summary>
        /// Test the CheckCurrentUserIsAdministratorAsync should check only the administrators action code.
        /// </summary>
        [Fact]
        public async Task CheckCurrentUserIsAdministratorAsync_Pass_ChecksOnlyTheAdministratorsActionCode()
        {
            // Arrange
            IBaseUsersRepository<ActionEntityFake, long> repository = SecurityHandlerFactory.CreateRepository(true);

            ISecurityHandler handler = SecurityHandlerFactory.Create(
                repository,
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID));

            // Act
            await handler.CheckCurrentUserIsAdministratorAsync();

            // Assert
            await repository.Received(1).HasAnyActionAsync(UserID, Arg.Is<string[]>(x => x.SequenceEqual(new[] { AdministratorsActionCode })));
        }

        /// <summary>
        /// Test the CheckCurrentUserIsAdministratorAsync should use the overridden administrators action code.
        /// </summary>
        [Fact]
        public async Task CheckCurrentUserIsAdministratorAsync_Pass_UsesTheOverriddenAdministratorsActionCode()
        {
            // Arrange
            IBaseUsersRepository<ActionEntityFake, long> repository = SecurityHandlerFactory.CreateRepository(true);

            ISecurityHandler handler = new SecurityHandlerFake(
                repository,
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID));

            // Act
            await handler.CheckCurrentUserIsAdministratorAsync();

            // Assert
            await repository.Received(1).HasAnyActionAsync(
                UserID,
                Arg.Is<string[]>(x => x.SequenceEqual(new[] { SecurityHandlerFake.CustomAdministratorsActionCode })));
        }
        #endregion

        #region CheckCurrentUserHasPermissionOrIsAdministratorAsync
        /// <summary>
        /// Test the CheckCurrentUserHasPermissionOrIsAdministratorAsync should return true when the repository confirms.
        /// </summary>
        [Fact]
        public async Task CheckCurrentUserHasPermissionOrIsAdministratorAsync_Pass_ReturnsTrueWhenTheUserHasTheAction()
        {
            // Arrange
            ISecurityHandler handler = SecurityHandlerFactory.Create(
                SecurityHandlerFactory.CreateRepository(true),
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID));

            // Act
            bool result = await handler.CheckCurrentUserHasPermissionOrIsAdministratorAsync("ActionFake");

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Test the CheckCurrentUserHasPermissionOrIsAdministratorAsync should return false when the repository denies.
        /// </summary>
        [Fact]
        public async Task CheckCurrentUserHasPermissionOrIsAdministratorAsync_Pass_ReturnsFalseWhenTheUserHasNeitherAction()
        {
            // Arrange
            ISecurityHandler handler = SecurityHandlerFactory.Create(
                SecurityHandlerFactory.CreateRepository(false),
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID));

            // Act
            bool result = await handler.CheckCurrentUserHasPermissionOrIsAdministratorAsync("ActionFake");

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Test the CheckCurrentUserHasPermissionOrIsAdministratorAsync should return false when there is no current user.
        /// </summary>
        [Fact]
        public async Task CheckCurrentUserHasPermissionOrIsAdministratorAsync_Fail_ReturnsFalseWhenThereIsNoCurrentUser()
        {
            // Arrange
            IBaseUsersRepository<ActionEntityFake, long> repository = SecurityHandlerFactory.CreateRepository(true);

            ISecurityHandler handler = SecurityHandlerFactory.Create(
                repository,
                SecurityHandlerFactory.CreateCurrentUserProvider(null));

            // Act
            bool result = await handler.CheckCurrentUserHasPermissionOrIsAdministratorAsync("ActionFake");

            // Assert
            result.Should().BeFalse();

            await repository.DidNotReceiveWithAnyArgs().HasAnyActionAsync(default, default!);
        }

        /// <summary>
        /// Test the CheckCurrentUserHasPermissionOrIsAdministratorAsync should check the administrators code and the action name.
        /// </summary>
        [Fact]
        public async Task CheckCurrentUserHasPermissionOrIsAdministratorAsync_Pass_ChecksTheAdministratorsCodeAndTheActionName()
        {
            // Arrange
            string actionName = "ActionFake";

            IBaseUsersRepository<ActionEntityFake, long> repository = SecurityHandlerFactory.CreateRepository(true);

            ISecurityHandler handler = SecurityHandlerFactory.Create(
                repository,
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID));

            // Act
            await handler.CheckCurrentUserHasPermissionOrIsAdministratorAsync(actionName);

            // Assert
            await repository.Received(1).HasAnyActionAsync(
                UserID,
                Arg.Is<string[]>(x => x.SequenceEqual(new[] { AdministratorsActionCode, actionName })));
        }

        /// <summary>
        /// Test the CheckCurrentUserHasPermissionOrIsAdministratorAsync should forward action names with no content.
        /// </summary>
        /// <param name="actionName">The action name to check.</param>
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task CheckCurrentUserHasPermissionOrIsAdministratorAsync_Pass_ForwardsActionNamesWithoutContent(string actionName)
        {
            // Arrange
            IBaseUsersRepository<ActionEntityFake, long> repository = SecurityHandlerFactory.CreateRepository(false);

            ISecurityHandler handler = SecurityHandlerFactory.Create(
                repository,
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID));

            // Act
            bool result = await handler.CheckCurrentUserHasPermissionOrIsAdministratorAsync(actionName);

            // Assert
            result.Should().BeFalse();

            await repository.Received(1).HasAnyActionAsync(UserID, Arg.Is<string[]>(x => x.Contains(actionName)));
        }

        /// <summary>
        /// Test the CheckCurrentUserHasPermissionOrIsAdministratorAsync should use the overridden administrators action code.
        /// </summary>
        [Fact]
        public async Task CheckCurrentUserHasPermissionOrIsAdministratorAsync_Pass_UsesTheOverriddenAdministratorsActionCode()
        {
            // Arrange
            IBaseUsersRepository<ActionEntityFake, long> repository = SecurityHandlerFactory.CreateRepository(true);

            ISecurityHandler handler = new SecurityHandlerFake(
                repository,
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID));

            // Act
            await handler.CheckCurrentUserHasPermissionOrIsAdministratorAsync("ActionFake");

            // Assert
            await repository.Received(1).HasAnyActionAsync(
                UserID,
                Arg.Is<string[]>(x => x.Contains(SecurityHandlerFake.CustomAdministratorsActionCode)));
        }
        #endregion

        #region ValidateUserHasPermissionAsync
        /// <summary>
        /// Test the ValidateUserHasPermissionAsync should throw when no method in the stack trace
        /// carries the action method attribute.
        /// </summary>
        [Fact]
        public async Task ValidateUserHasPermissionAsync_Fail_ThrowsWhenNoMethodInTheStackTraceIsAnAction()
        {
            // Arrange
            ISecurityHandler handler = SecurityHandlerFactory.Create(
                SecurityHandlerFactory.CreateRepository(true),
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID));

            // Act
            Func<Task> act = async () => await handler.ValidateUserHasPermissionAsync();

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Could not find any method implementing the ActionMethodAttribute in the stack trace.");
        }

        /// <summary>
        /// Test the ValidateUserHasPermissionAsync should throw when there is no current user.
        /// </summary>
        [Fact]
        public async Task ValidateUserHasPermissionAsync_Fail_ThrowsWhenThereIsNoCurrentUser()
        {
            // Arrange
            IBaseUsersRepository<ActionEntityFake, long> repository = SecurityHandlerFactory.CreateRepository(true);

            ISecuredServiceFake service = new SecuredServiceFake(SecurityHandlerFactory.Create(
                repository,
                SecurityHandlerFactory.CreateCurrentUserProvider(null)));

            // Act
            Func<Task> act = async () => await service.DefaultActionAsync();

            // Assert
            await act.Should().ThrowAsync<MissingUserPermissionException>()
                .WithMessage($"The current user is missing permissions to action '{nameof(ISecuredServiceFake)}.{nameof(ISecuredServiceFake.DefaultActionAsync)}'.");

            await repository.DidNotReceiveWithAnyArgs().HasAnyActionAsync(default, default!);
        }

        /// <summary>
        /// Test the ValidateUserHasPermissionAsync should throw when the user has none of the checked actions.
        /// </summary>
        [Fact]
        public async Task ValidateUserHasPermissionAsync_Fail_ThrowsWhenTheUserHasNoneOfTheActions()
        {
            // Arrange
            ISecuredServiceFake service = new SecuredServiceFake(SecurityHandlerFactory.Create(
                SecurityHandlerFactory.CreateRepository(false),
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID)));

            // Act
            Func<Task> act = async () => await service.DefaultActionAsync();

            // Assert
            await act.Should().ThrowAsync<MissingUserPermissionException>()
                .WithMessage($"The current user is missing permissions to action '{nameof(ISecuredServiceFake)}.{nameof(ISecuredServiceFake.DefaultActionAsync)}'.");
        }

        /// <summary>
        /// Test the ValidateUserHasPermissionAsync should not throw when the user has one of the checked actions.
        /// </summary>
        [Fact]
        public async Task ValidateUserHasPermissionAsync_Pass_DoesNotThrowWhenTheUserHasTheAction()
        {
            // Arrange
            ISecuredServiceFake service = new SecuredServiceFake(SecurityHandlerFactory.Create(
                SecurityHandlerFactory.CreateRepository(true),
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID)));

            // Act
            Func<Task> act = async () => await service.DefaultActionAsync();

            // Assert
            await act.Should().NotThrowAsync();
        }

        /// <summary>
        /// Test the ValidateUserHasPermissionAsync should build the action name from the first
        /// interface of the service and the method name.
        /// </summary>
        [Fact]
        public async Task ValidateUserHasPermissionAsync_Pass_BuildsTheActionNameFromTheServiceInterfaceAndTheMethod()
        {
            // Arrange
            IBaseUsersRepository<ActionEntityFake, long> repository = SecurityHandlerFactory.CreateRepository(true);

            ISecuredServiceFake service = new SecuredServiceFake(SecurityHandlerFactory.Create(
                repository,
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID)));

            // Act
            await service.DefaultActionAsync();

            // Assert
            await repository.Received(1).HasAnyActionAsync(
                UserID,
                Arg.Is<string[]>(x => x.Contains($"{nameof(ISecuredServiceFake)}.{nameof(ISecuredServiceFake.DefaultActionAsync)}")));
        }

        /// <summary>
        /// Test the ValidateUserHasPermissionAsync should leave the service part of the action name
        /// empty when the service implements no interface.
        /// </summary>
        [Fact]
        public async Task ValidateUserHasPermissionAsync_Pass_BuildsTheActionNameWithoutServiceWhenThereIsNoInterface()
        {
            // Arrange
            IBaseUsersRepository<ActionEntityFake, long> repository = SecurityHandlerFactory.CreateRepository(true);

            InterfacelessServiceFake service = new(SecurityHandlerFactory.Create(
                repository,
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID)));

            // Act
            await service.DefaultActionAsync();

            // Assert
            await repository.Received(1).HasAnyActionAsync(
                UserID,
                Arg.Is<string[]>(x => x.Contains($".{nameof(InterfacelessServiceFake.DefaultActionAsync)}")));
        }

        /// <summary>
        /// Test the ValidateUserHasPermissionAsync should check the administrators action code for
        /// an action restricted to administrators.
        /// </summary>
        [Fact]
        public async Task ValidateUserHasPermissionAsync_Pass_ChecksTheAdministratorsCodeForAnAdministratorsAction()
        {
            // Arrange
            IBaseUsersRepository<ActionEntityFake, long> repository = SecurityHandlerFactory.CreateRepository(true);

            ISecuredServiceFake service = new SecuredServiceFake(SecurityHandlerFactory.Create(
                repository,
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID)));

            // Act
            await service.OnlyAdminsActionAsync();

            // Assert
            await repository.Received(1).HasAnyActionAsync(UserID, Arg.Is<string[]>(x => x.Contains(AdministratorsActionCode)));
        }

        /// <summary>
        /// Test the ValidateUserHasPermissionAsync should throw for an administrators action when the
        /// user has none of the checked actions.
        /// </summary>
        [Fact]
        public async Task ValidateUserHasPermissionAsync_Fail_ThrowsForAnAdministratorsActionWhenTheUserHasNoAction()
        {
            // Arrange
            ISecuredServiceFake service = new SecuredServiceFake(SecurityHandlerFactory.Create(
                SecurityHandlerFactory.CreateRepository(false),
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID)));

            // Act
            Func<Task> act = async () => await service.OnlyAdminsActionAsync();

            // Assert
            await act.Should().ThrowAsync<MissingUserPermissionException>()
                .WithMessage($"The current user is missing permissions to action '{nameof(ISecuredServiceFake)}.{nameof(ISecuredServiceFake.OnlyAdminsActionAsync)}'.");
        }

        /// <summary>
        /// Test the ValidateUserHasPermissionAsync should not check the administrators action code
        /// for an action restricted to regular users.
        /// </summary>
        [Fact]
        public async Task ValidateUserHasPermissionAsync_Pass_DoesNotCheckTheAdministratorsCodeForARegularUsersAction()
        {
            // Arrange
            IBaseUsersRepository<ActionEntityFake, long> repository = SecurityHandlerFactory.CreateRepository(true);

            ISecuredServiceFake service = new SecuredServiceFake(SecurityHandlerFactory.Create(
                repository,
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID)));

            // Act
            await service.OnlyRegularUsersActionAsync();

            // Assert
            await repository.Received(1).HasAnyActionAsync(
                UserID,
                Arg.Is<string[]>(x =>
                    !x.Contains(AdministratorsActionCode)
                    && x.Contains($"{nameof(ISecuredServiceFake)}.{nameof(ISecuredServiceFake.OnlyRegularUsersActionAsync)}")));
        }

        /// <summary>
        /// Test the ValidateUserHasPermissionAsync should check both the action name and the
        /// administrators action code for an action open to regular users and administrators.
        /// </summary>
        [Fact]
        public async Task ValidateUserHasPermissionAsync_Pass_ChecksTheActionNameAndTheAdministratorsCodeForADefaultAction()
        {
            // Arrange
            IBaseUsersRepository<ActionEntityFake, long> repository = SecurityHandlerFactory.CreateRepository(true);

            ISecuredServiceFake service = new SecuredServiceFake(SecurityHandlerFactory.Create(
                repository,
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID)));

            // Act
            await service.DefaultActionAsync();

            // Assert
            await repository.Received(1).HasAnyActionAsync(
                UserID,
                Arg.Is<string[]>(x =>
                    x.Contains(AdministratorsActionCode)
                    && x.Contains($"{nameof(ISecuredServiceFake)}.{nameof(ISecuredServiceFake.DefaultActionAsync)}")));
        }

        /// <summary>
        /// Test the ValidateUserHasPermissionAsync should use the overridden administrators action code.
        /// </summary>
        [Fact]
        public async Task ValidateUserHasPermissionAsync_Pass_UsesTheOverriddenAdministratorsActionCode()
        {
            // Arrange
            IBaseUsersRepository<ActionEntityFake, long> repository = SecurityHandlerFactory.CreateRepository(true);

            ISecuredServiceFake service = new SecuredServiceFake(new SecurityHandlerFake(
                repository,
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID)));

            // Act
            await service.DefaultActionAsync();

            // Assert
            await repository.Received(1).HasAnyActionAsync(
                UserID,
                Arg.Is<string[]>(x =>
                    x.Contains(SecurityHandlerFake.CustomAdministratorsActionCode)
                    && !x.Contains(AdministratorsActionCode)));
        }

        /// <summary>
        /// Test the ValidateUserHasPermissionAsync should find the action further down the stack
        /// trace when the entry point itself is not an action.
        /// </summary>
        [Fact]
        public async Task ValidateUserHasPermissionAsync_Pass_FindsTheActionFurtherDownTheStackTrace()
        {
            // Arrange
            IBaseUsersRepository<ActionEntityFake, long> repository = SecurityHandlerFactory.CreateRepository(true);

            ISecuredServiceFake service = new SecuredServiceFake(SecurityHandlerFactory.Create(
                repository,
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID)));

            // Act
            await service.NestedCallActionAsync();

            // Assert
            await repository.Received(1).HasAnyActionAsync(
                UserID,
                Arg.Is<string[]>(x => x.Contains($"{nameof(ISecuredServiceFake)}.{nameof(ISecuredServiceFake.DefaultActionAsync)}")));
        }

        /// <summary>
        /// Test the ValidateUserHasPermissionAsync should throw when the calling method is not an action.
        /// </summary>
        [Fact]
        public async Task ValidateUserHasPermissionAsync_Fail_ThrowsWhenTheCallingMethodIsNotAnAction()
        {
            // Arrange
            ISecuredServiceFake service = new SecuredServiceFake(SecurityHandlerFactory.Create(
                SecurityHandlerFactory.CreateRepository(true),
                SecurityHandlerFactory.CreateCurrentUserProvider(UserID)));

            // Act
            Func<Task> act = async () => await service.UnattributedActionAsync();

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Could not find any method implementing the ActionMethodAttribute in the stack trace.");
        }
        #endregion
    }
}
