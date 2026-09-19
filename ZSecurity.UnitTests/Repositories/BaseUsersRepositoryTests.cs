using ZDatabase.Interfaces;
using ZSecurity.UnitTests.Fakes.Repositories;

namespace ZSecurity.UnitTests.Repositories
{
    /// <summary>
    /// Unit tests for <see cref="ZSecurity.Repositories.BaseUsersRepository{TActions, TUsersKey}"/>.
    /// </summary>
    public class BaseUsersRepositoryTests
    {
        private const long UserID = 42;

        /// <summary>
        /// Test the constructor should keep the database context it received.
        /// </summary>
        [Fact]
        public void Constructor_Pass_KeepsTheDbContext()
        {
            // Arrange
            IDbContext dbContext = Substitute.For<IDbContext>();

            // Act
            UsersRepositoryFake repository = new(dbContext);

            // Assert
            repository.ExposedDbContext.Should().BeSameAs(dbContext);
        }

        /// <summary>
        /// Test the HasAnyActionAsync should throw when no action code is provided.
        /// </summary>
        [Fact]
        public async Task HasAnyActionAsync_Fail_ThrowsWhenNoActionCodeIsProvided()
        {
            // Arrange
            UsersRepositoryFake repository = new(Substitute.For<IDbContext>(), "ActionFake");

            // Act
            Func<Task> act = async () => await repository.HasAnyActionAsync(UserID);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithParameterName("actionCodes");
        }

        /// <summary>
        /// Test the HasAnyActionAsync should throw when the action codes array is empty.
        /// </summary>
        [Fact]
        public async Task HasAnyActionAsync_Fail_ThrowsWhenTheActionCodesArrayIsEmpty()
        {
            // Arrange
            UsersRepositoryFake repository = new(Substitute.For<IDbContext>(), "ActionFake");

            // Act
            Func<Task> act = async () => await repository.HasAnyActionAsync(UserID, []);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithParameterName("actionCodes");
        }

        /// <summary>
        /// Test the HasAnyActionAsync should not query the actions when no action code is provided.
        /// </summary>
        [Fact]
        public async Task HasAnyActionAsync_Fail_DoesNotListTheActionsWhenNoActionCodeIsProvided()
        {
            // Arrange
            UsersRepositoryFake repository = new(Substitute.For<IDbContext>(), "ActionFake");

            // Act
            Func<Task> act = async () => await repository.HasAnyActionAsync(UserID);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();

            repository.LastRequestedUserID.Should().BeNull();
        }

        /// <summary>
        /// Test the HasAnyActionAsync should return true when the user has the requested action.
        /// </summary>
        [Fact]
        public async Task HasAnyActionAsync_Pass_ReturnsTrueWhenTheUserHasTheAction()
        {
            // Arrange
            UsersRepositoryFake repository = new(Substitute.For<IDbContext>(), "ActionFake");

            // Act
            bool result = await repository.HasAnyActionAsync(UserID, "ActionFake");

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Test the HasAnyActionAsync should return true when the user has at least one of the requested actions.
        /// </summary>
        [Fact]
        public async Task HasAnyActionAsync_Pass_ReturnsTrueWhenTheUserHasAnyOfTheActions()
        {
            // Arrange
            UsersRepositoryFake repository = new(Substitute.For<IDbContext>(), "SecondActionFake");

            // Act
            bool result = await repository.HasAnyActionAsync(UserID, "FirstActionFake", "SecondActionFake", "ThirdActionFake");

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Test the HasAnyActionAsync should return false when none of the requested actions match.
        /// </summary>
        [Fact]
        public async Task HasAnyActionAsync_Pass_ReturnsFalseWhenNoneOfTheActionsMatch()
        {
            // Arrange
            UsersRepositoryFake repository = new(Substitute.For<IDbContext>(), "FirstActionFake", "SecondActionFake");

            // Act
            bool result = await repository.HasAnyActionAsync(UserID, "ThirdActionFake");

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Test the HasAnyActionAsync should return false when the user has no action at all.
        /// </summary>
        [Fact]
        public async Task HasAnyActionAsync_Pass_ReturnsFalseWhenTheUserHasNoAction()
        {
            // Arrange
            UsersRepositoryFake repository = new(Substitute.For<IDbContext>());

            // Act
            bool result = await repository.HasAnyActionAsync(UserID, "ActionFake");

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Test the HasAnyActionAsync should not match actions whose code is null.
        /// </summary>
        [Fact]
        public async Task HasAnyActionAsync_Pass_DoesNotMatchActionsWithoutCode()
        {
            // Arrange
            UsersRepositoryFake repository = new(Substitute.For<IDbContext>(), [null, string.Empty]);

            // Act
            bool result = await repository.HasAnyActionAsync(UserID, "ActionFake");

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Test the HasAnyActionAsync should match the action code exactly, respecting the casing.
        /// </summary>
        /// <param name="actionCode">The action code to look for.</param>
        /// <param name="expected">Whether the user is expected to have the action.</param>
        [Theory]
        [InlineData("ActionFake", true)]
        [InlineData("actionfake", false)]
        [InlineData("ACTIONFAKE", false)]
        [InlineData(" ActionFake", false)]
        public async Task HasAnyActionAsync_Pass_MatchesTheActionCodeExactly(string actionCode, bool expected)
        {
            // Arrange
            UsersRepositoryFake repository = new(Substitute.For<IDbContext>(), "ActionFake");

            // Act
            bool result = await repository.HasAnyActionAsync(UserID, actionCode);

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Test the HasAnyActionAsync should forward the user identifier to the actions listing.
        /// </summary>
        [Fact]
        public async Task HasAnyActionAsync_Pass_ForwardsTheUserIDToListAllActions()
        {
            // Arrange
            UsersRepositoryFake repository = new(Substitute.For<IDbContext>(), "ActionFake");

            // Act
            await repository.HasAnyActionAsync(UserID, "ActionFake");

            // Assert
            repository.LastRequestedUserID.Should().Be(UserID);
        }

        /// <summary>
        /// Test the ListAllActionsAsync should return every action of the user.
        /// </summary>
        [Fact]
        public async Task ListAllActionsAsync_Pass_ReturnsAllTheUserActions()
        {
            // Arrange
            UsersRepositoryFake repository = new(Substitute.For<IDbContext>(), "FirstActionFake", "SecondActionFake");

            // Act
            IEnumerable<Fakes.Entities.ActionEntityFake> actions = await repository.ListAllActionsAsync(UserID);

            // Assert
            actions.Should().HaveCount(2);
            actions.Select(x => x.Code).Should().BeEquivalentTo(["FirstActionFake", "SecondActionFake"]);
        }
    }
}
