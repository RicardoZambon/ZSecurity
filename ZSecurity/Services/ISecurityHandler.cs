namespace ZSecurity.Services
{
    /// <summary>
    /// Handler to check user has permission or is administrator.
    /// </summary>
    public interface ISecurityHandler
    {
        /// <summary>
        /// Checks the current user has permission or is administrator asynchronous.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <returns>
        ///     <c>true</c> if the user has the permission or is administrator; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> CheckCurrentUserHasPermissionOrIsAdministratorAsync(string actionName);

        /// <summary>
        /// Checks the current user is administrator asynchronous.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the user is administrator; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> CheckCurrentUserIsAdministratorAsync();

        /// <summary>
        /// Validates the user has permission asynchronous. A previous method must implement the <see cref="ZSecurity.Attributes.ActionMethodAttribute"/> attribute.
        /// </summary>
        /// <exception cref="System.Exception">Could not find action method.</exception>
        /// <exception cref="ZSecurity.Exceptions.MissingUserPermissionException">The user is missing the action permission.</exception>
        Task ValidateUserHasPermissionAsync();
    }
}