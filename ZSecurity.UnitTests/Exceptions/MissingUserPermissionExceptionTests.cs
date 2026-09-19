using ZSecurity.Exceptions;

namespace ZSecurity.UnitTests.Exceptions
{
    /// <summary>
    /// Unit tests for <see cref="ZSecurity.Exceptions.MissingUserPermissionException"/>.
    /// </summary>
    public class MissingUserPermissionExceptionTests
    {
        /// <summary>
        /// Test the constructor should build the message from the action name.
        /// </summary>
        [Fact]
        public void Constructor_Pass_BuildsMessageFromActionName()
        {
            // Arrange
            string actionName = "IServiceFake.ActionFake";

            // Act
            MissingUserPermissionException exception = new(actionName);

            // Assert
            exception.Message.Should().Be($"The current user is missing permissions to action '{actionName}'.");
        }

        /// <summary>
        /// Test the constructor should accept action names with no content.
        /// </summary>
        /// <param name="actionName">The action name to set.</param>
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(".")]
        public void Constructor_Pass_AcceptsActionNamesWithoutContent(string actionName)
        {
            // Arrange

            // Act
            MissingUserPermissionException exception = new(actionName);

            // Assert
            exception.Message.Should().Be($"The current user is missing permissions to action '{actionName}'.");
        }

        /// <summary>
        /// Test the constructor should accept a null action name.
        /// </summary>
        [Fact]
        public void Constructor_Pass_AcceptsNullActionName()
        {
            // Arrange
            string? actionName = null;

            // Act
            MissingUserPermissionException exception = new(actionName!);

            // Assert
            exception.Message.Should().Be("The current user is missing permissions to action ''.");
        }

        /// <summary>
        /// Test the exception should be throwable and carry no inner exception.
        /// </summary>
        [Fact]
        public void Constructor_Fail_ThrowsAsAnExceptionWithoutInnerException()
        {
            // Arrange
            string actionName = "IServiceFake.ActionFake";

            // Act
            Action act = () => throw new MissingUserPermissionException(actionName);

            // Assert
            act.Should().Throw<MissingUserPermissionException>()
                .WithMessage($"The current user is missing permissions to action '{actionName}'.")
                .And.InnerException.Should().BeNull();
        }
    }
}
