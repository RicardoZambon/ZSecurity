namespace ZSecurity.Exceptions
{
    /// <summary>
    /// Exception thrown when a user is missing permissions to perform an action.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class MissingUserPermissionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingUserPermissionException"/> class.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        public MissingUserPermissionException(string actionName) : base($"The current user is missing permissions to action '{actionName}'.")
        {
        }
    }
}