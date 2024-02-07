using System.Diagnostics;
using System.Reflection;
using ZWebApi.Attributes;

namespace ZWebApi.Helpers
{
    /// <summary>
    /// Helper static class for <see cref="ZWebApi.Attributes.ActionMethodAttribute"/>.
    /// </summary>
    public static class StackTraceHelper
    {
        /// <summary>
        /// Gets the method implementing the <see cref="ZWebApi.Attributes.ActionMethodAttribute"/>.
        /// </summary>
        /// <returns>If found, the method that implements the <see cref="Niten.System.Core.Helpers.Attributes.ActionMethodAttribute"/>; otherwise, <c>null</c>.</returns>
        public static MethodBase? GetMethodImplementingActionMethodAttribute()
        {
            StackTrace stackTrace = new();
            foreach (StackFrame stackFrame in stackTrace.GetFrames())
            {
                MethodBase? method = stackFrame.GetMethod();

                if (method?.GetCustomAttribute<ActionMethodAttribute>() is not null)
                {
                    return method;
                }
            }
            return null;
        }
    }
}