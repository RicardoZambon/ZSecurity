namespace ZSecurity.UnitTests.Fakes.Attributes
{
    /// <summary>
    /// An attribute that is deliberately never applied to any method, so that a stack
    /// trace lookup for it is guaranteed to find nothing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    internal sealed class UnusedMethodAttributeFake : Attribute
    {
    }
}
