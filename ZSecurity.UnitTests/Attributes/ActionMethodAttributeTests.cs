using ZSecurity.Attributes;
using ZSecurity.Enums;

namespace ZSecurity.UnitTests.Attributes
{
    /// <summary>
    /// Unit tests for <see cref="ZSecurity.Attributes.ActionMethodAttribute"/>.
    /// </summary>
    public class ActionMethodAttributeTests
    {
        /// <summary>
        /// Test the constructor should default the action type to regular users and administrators.
        /// </summary>
        [Fact]
        public void Constructor_Pass_DefaultsActionTypeToRegularUsersAndAdmins()
        {
            // Arrange

            // Act
            ActionMethodAttribute attribute = new();

            // Assert
            attribute.ActionType.Should().Be(ActionTypes.RegularUsersAndAdmins);
        }

        /// <summary>
        /// Test the constructor should keep the action type it received.
        /// </summary>
        /// <param name="actionType">The action type to set.</param>
        [Theory]
        [InlineData(ActionTypes.RegularUsersAndAdmins)]
        [InlineData(ActionTypes.OnlyAdmins)]
        [InlineData(ActionTypes.OnlyRegularUsers)]
        public void Constructor_Pass_KeepsTheProvidedActionType(ActionTypes actionType)
        {
            // Arrange

            // Act
            ActionMethodAttribute attribute = new(actionType);

            // Assert
            attribute.ActionType.Should().Be(actionType);
        }

        /// <summary>
        /// Test the constructor should keep an action type outside the declared enum range.
        /// </summary>
        [Fact]
        public void Constructor_Pass_KeepsAnUndefinedActionType()
        {
            // Arrange
            ActionTypes undefinedActionType = (ActionTypes)99;

            // Act
            ActionMethodAttribute attribute = new(undefinedActionType);

            // Assert
            attribute.ActionType.Should().Be(undefinedActionType);
            Enum.IsDefined(attribute.ActionType).Should().BeFalse();
        }

        /// <summary>
        /// Test the attribute should only be applicable once, to methods, without inheritance.
        /// </summary>
        [Fact]
        public void AttributeUsage_Pass_TargetsMethodsOnlyOnceWithoutInheritance()
        {
            // Arrange

            // Act
            AttributeUsageAttribute? usage = typeof(ActionMethodAttribute)
                .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
                .Cast<AttributeUsageAttribute>()
                .FirstOrDefault();

            // Assert
            usage.Should().NotBeNull();
            usage!.ValidOn.Should().Be(AttributeTargets.Method);
            usage.Inherited.Should().BeFalse();
            usage.AllowMultiple.Should().BeFalse();
        }
    }
}
