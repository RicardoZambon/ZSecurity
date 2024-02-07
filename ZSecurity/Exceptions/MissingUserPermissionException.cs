namespace ZSecurity.Exceptions
{
    public class MissingUserPermissionException : Exception
    {
        public MissingUserPermissionException(string actionName) : base($"The current user is missing permissions to action '{actionName}'.")
        {
        }
    }
}