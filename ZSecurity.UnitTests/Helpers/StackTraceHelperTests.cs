using System.Reflection;
using System.Runtime.CompilerServices;
using ZSecurity.Attributes;
using ZSecurity.Enums;
using ZSecurity.Helpers;
using ZSecurity.UnitTests.Fakes.Attributes;
using ZSecurity.UnitTests.Fakes.Helpers;

namespace ZSecurity.UnitTests.Helpers
{
    /// <summary>
    /// Unit tests for <see cref="ZSecurity.Helpers.StackTraceHelper"/>.
    /// </summary>
    public class StackTraceHelperTests
    {
        /// <summary>
        /// Test the GetStackTrace should include the synchronous method that called it.
        /// </summary>
        [Fact]
        public void GetStackTrace_Pass_IncludesTheCallingSynchronousMethod()
        {
            // Arrange
            StackTraceFake fake = new();

            // Act
            IEnumerable<MethodBase> stackTrace = fake.CollectSync();

            // Assert
            stackTrace.Should().Contain(x => x.Name == nameof(StackTraceFake.CollectSync) && x.DeclaringType == typeof(StackTraceFake));
        }

        /// <summary>
        /// Test the GetStackTrace should include the test method that called it directly.
        /// </summary>
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void GetStackTrace_Pass_IncludesTheDirectCaller()
        {
            // Arrange

            // Act
            IEnumerable<MethodBase> stackTrace = StackTraceHelper.GetStackTrace();

            // Assert
            stackTrace.Should().Contain(x => x.Name == nameof(GetStackTrace_Pass_IncludesTheDirectCaller));
        }

        /// <summary>
        /// Test the GetStackTrace should never return a frame without a declaring type.
        /// </summary>
        [Fact]
        public void GetStackTrace_Pass_SkipsFramesWithoutDeclaringType()
        {
            // Arrange

            // Act
            IEnumerable<MethodBase> stackTrace = StackTraceHelper.GetStackTrace();

            // Assert
            stackTrace.Should().NotBeEmpty();
            stackTrace.Should().OnlyContain(x => x.DeclaringType != null);
        }

        /// <summary>
        /// Test the GetStackTrace should resolve an async state machine back to the async method.
        /// </summary>
        [Fact]
        public async Task GetStackTrace_Pass_ResolvesTheAsyncMethodFromItsStateMachine()
        {
            // Arrange
            StackTraceFake fake = new();

            // Act
            IEnumerable<MethodBase> stackTrace = await fake.CollectAsync();

            // Assert
            stackTrace.Should().Contain(x => x.Name == nameof(StackTraceFake.CollectAsync) && x.DeclaringType == typeof(StackTraceFake));
            stackTrace.Should().NotContain(x => x.Name == "MoveNext" && x.DeclaringType!.DeclaringType == typeof(StackTraceFake));
        }

        /// <summary>
        /// Test the GetStackTrace should resolve the async overload whose parameter names match the state machine.
        /// </summary>
        [Fact]
        public async Task GetStackTrace_Pass_ResolvesTheAsyncOverloadWithTheIntegerParameter()
        {
            // Arrange
            StackTraceFake fake = new();

            // Act
            IEnumerable<MethodBase> stackTrace = await fake.CollectOverloadAsync(1);

            // Assert
            MethodBase? resolved = stackTrace.FirstOrDefault(x => x.Name == nameof(StackTraceFake.CollectOverloadAsync));

            resolved.Should().NotBeNull();
            resolved!.GetParameters().Should().ContainSingle();
            resolved.GetParameters()[0].Name.Should().Be("numberValue");
        }

        /// <summary>
        /// Test the GetStackTrace should resolve the async overload whose parameter names match the state machine.
        /// </summary>
        [Fact]
        public async Task GetStackTrace_Pass_ResolvesTheAsyncOverloadWithTheStringParameter()
        {
            // Arrange
            StackTraceFake fake = new();

            // Act
            IEnumerable<MethodBase> stackTrace = await fake.CollectOverloadAsync("text");

            // Assert
            MethodBase? resolved = stackTrace.FirstOrDefault(x => x.Name == nameof(StackTraceFake.CollectOverloadAsync));

            resolved.Should().NotBeNull();
            resolved!.GetParameters().Should().ContainSingle();
            resolved.GetParameters()[0].Name.Should().Be("textValue");
        }

        /// <summary>
        /// Test the GetStackTrace should fall back to the compiler generated frame when the async
        /// method behind the state machine cannot be resolved, as happens for async lambdas.
        /// </summary>
        [Fact]
        public async Task GetStackTrace_Pass_KeepsTheStateMachineFrameWhenTheAsyncMethodCannotBeResolved()
        {
            // Arrange
            StackTraceFake fake = new();

            // Act
            IEnumerable<MethodBase> stackTrace = await fake.CollectFromAsyncLambdaAsync();

            // Assert
            stackTrace.Should().Contain(x =>
                x.Name == "MoveNext"
                && x.DeclaringType!.Name.Contains(nameof(StackTraceFake.CollectFromAsyncLambdaAsync)));
        }

        /// <summary>
        /// Test the GetStackTraceMethodImplementingAttribute should return the synchronous method carrying the attribute.
        /// </summary>
        [Fact]
        public void GetStackTraceMethodImplementingAttribute_Pass_ReturnsTheSynchronousAttributedMethod()
        {
            // Arrange
            StackTraceFake fake = new();

            // Act
            MethodBase? method = fake.FindAttributedSync();

            // Assert
            method.Should().NotBeNull();
            method!.Name.Should().Be(nameof(StackTraceFake.FindAttributedSync));
            method.DeclaringType.Should().Be<StackTraceFake>();
            method.GetCustomAttribute<ActionMethodAttribute>()!.ActionType.Should().Be(ActionTypes.OnlyAdmins);
        }

        /// <summary>
        /// Test the GetStackTraceMethodImplementingAttribute should return the async method carrying the attribute.
        /// </summary>
        [Fact]
        public async Task GetStackTraceMethodImplementingAttribute_Pass_ReturnsTheAsyncAttributedMethod()
        {
            // Arrange
            StackTraceFake fake = new();

            // Act
            MethodBase? method = await fake.FindAttributedAsync();

            // Assert
            method.Should().NotBeNull();
            method!.Name.Should().Be(nameof(StackTraceFake.FindAttributedAsync));
            method.DeclaringType.Should().Be<StackTraceFake>();
            method.GetCustomAttribute<ActionMethodAttribute>()!.ActionType.Should().Be(ActionTypes.RegularUsersAndAdmins);
        }

        /// <summary>
        /// Test the GetStackTraceMethodImplementingAttribute should walk past unattributed frames.
        /// </summary>
        [Fact]
        public void GetStackTraceMethodImplementingAttribute_Pass_WalksPastUnattributedFrames()
        {
            // Arrange
            StackTraceFake fake = new();

            // Act
            MethodBase? method = fake.FindAttributedFromNestedCall();

            // Assert
            method.Should().NotBeNull();
            method!.Name.Should().Be(nameof(StackTraceFake.FindAttributedSync));
        }

        /// <summary>
        /// Test the GetStackTraceMethodImplementingAttribute should return null when nothing in the
        /// stack trace carries the attribute.
        /// </summary>
        [Fact]
        public void GetStackTraceMethodImplementingAttribute_Fail_ReturnsNullWhenNoMethodCarriesTheAttribute()
        {
            // Arrange

            // Act
            MethodBase? method = StackTraceHelper.GetStackTraceMethodImplementingAttribute<UnusedMethodAttributeFake>();

            // Assert
            method.Should().BeNull();
        }

        /// <summary>
        /// Test the GetStackTraceMethodImplementingAttribute should return null when the attributed
        /// frame has already left the stack.
        /// </summary>
        [Fact]
        public void GetStackTraceMethodImplementingAttribute_Fail_ReturnsNullAfterTheAttributedFrameReturned()
        {
            // Arrange
            StackTraceFake fake = new();
            fake.FindAttributedSync();

            // Act
            MethodBase? method = StackTraceHelper.GetStackTraceMethodImplementingAttribute<ActionMethodAttribute>();

            // Assert
            method.Should().BeNull();
        }
    }
}
