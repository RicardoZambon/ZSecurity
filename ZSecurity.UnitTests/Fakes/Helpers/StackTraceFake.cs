using System.Reflection;
using System.Runtime.CompilerServices;
using ZSecurity.Attributes;
using ZSecurity.Enums;
using ZSecurity.Helpers;

namespace ZSecurity.UnitTests.Fakes.Helpers
{
    /// <summary>
    /// Stand-in caller used to observe what <see cref="StackTraceHelper"/> reports for the
    /// different shapes of frame it has to deal with: plain synchronous methods, async state
    /// machines, async overloads and async lambdas.
    /// </summary>
    internal sealed class StackTraceFake
    {
        /// <summary>
        /// A plain synchronous frame.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public IEnumerable<MethodBase> CollectSync()
        {
            return StackTraceHelper.GetStackTrace();
        }

        /// <summary>
        /// An async frame, so the helper has to resolve the compiler generated MoveNext back
        /// to this method.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public async Task<IEnumerable<MethodBase>> CollectAsync()
        {
            await Task.Yield();
            return StackTraceHelper.GetStackTrace();
        }

        /// <summary>
        /// An async overload taking an <see cref="int"/>, to exercise the parameter name matching.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public async Task<IEnumerable<MethodBase>> CollectOverloadAsync(int numberValue)
        {
            await Task.Yield();
            GC.KeepAlive(numberValue);
            return StackTraceHelper.GetStackTrace();
        }

        /// <summary>
        /// An async overload taking a <see cref="string"/>, to exercise the parameter name matching.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public async Task<IEnumerable<MethodBase>> CollectOverloadAsync(string textValue)
        {
            await Task.Yield();
            GC.KeepAlive(textValue);
            return StackTraceHelper.GetStackTrace();
        }

        /// <summary>
        /// An async lambda, whose state machine type name cannot be resolved back to a method.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public Task<IEnumerable<MethodBase>> CollectFromAsyncLambdaAsync()
        {
            Func<Task<IEnumerable<MethodBase>>> lambda = async () =>
            {
                await Task.Yield();
                return StackTraceHelper.GetStackTrace();
            };
            return lambda();
        }

        /// <summary>
        /// A synchronous frame carrying the <see cref="ActionMethodAttribute"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [ActionMethod(ActionTypes.OnlyAdmins)]
        public MethodBase? FindAttributedSync()
        {
            return StackTraceHelper.GetStackTraceMethodImplementingAttribute<ActionMethodAttribute>();
        }

        /// <summary>
        /// An async frame carrying the <see cref="ActionMethodAttribute"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [ActionMethod]
        public async Task<MethodBase?> FindAttributedAsync()
        {
            await Task.Yield();
            return StackTraceHelper.GetStackTraceMethodImplementingAttribute<ActionMethodAttribute>();
        }

        /// <summary>
        /// A frame without the <see cref="ActionMethodAttribute"/>, calling through to a
        /// caller that does carry it.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public MethodBase? FindAttributedFromNestedCall()
        {
            return FindAttributedSync();
        }
    }
}
