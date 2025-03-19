using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ZSecurity.Helpers
{
    /// <summary>
    /// Static helper class for <see cref="ZSecurity.Attributes.ActionMethodAttribute"/>.
    /// </summary>
    public static class StackTraceHelper
    {
        private const string ASYNC_MOVE_NEXT_METHOD_NAME = "MoveNext";

        /// <summary>
        /// Gets the method from the stack trace that implements the specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns>If found, return the method that implements the specified attribute; otherwise, <c>null</c>.</returns>
        public static MethodBase? GetStackTraceMethodImplementingAttribute<TAttribute>()
             where TAttribute : Attribute
        {
            IEnumerable<MethodBase> stackTrace = GetStackTrace();
            foreach (MethodBase method in stackTrace)
            {
                if (method.GetCustomAttribute<TAttribute>() is not null)
                {
                    return method;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the full stack trace.
        /// </summary>
        /// <returns>The method stack trace list.</returns>
        public static IEnumerable<MethodBase> GetStackTrace()
        {
            IList<MethodBase> fullStackTrace = [];
            StackTrace stackTrace = new();
            foreach (StackFrame stackFrame in stackTrace.GetFrames())
            {
                MethodBase? method = stackFrame.GetMethod();
                if (method is not null && method.DeclaringType is not null)
                {
                    if (method.Name == ASYNC_MOVE_NEXT_METHOD_NAME && method.DeclaringType.IsNested)
                    {
                        // This means the method is an async method and we need to get the related method from the declaring type.
                        MemberInfo[] asyncMethodMembers = method.DeclaringType.GetMembers();
                        string methodName = Regex.Match(method.DeclaringType.Name, @"(?<=<)[^>]+(?=>)").Value;

                        Type? declaringType = method.DeclaringType.DeclaringType;

                        // We need to match also para method parameters, in case the async method is an overload.
                        MethodBase? foundMethod = declaringType?.GetMethods()
                            .FirstOrDefault(
                                x => x.Name == methodName
                                && x.GetParameters().All(y => asyncMethodMembers.Any(z => z.Name == y.Name))
                            );

                        if (foundMethod is not null)
                        {
                            method = foundMethod;
                        }
                    }

                    fullStackTrace.Add(method);
                }
            }
            return fullStackTrace;
        }
    }
}