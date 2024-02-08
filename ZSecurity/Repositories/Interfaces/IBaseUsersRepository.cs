using ZSecurity.Interfaces;

namespace ZSecurity.Repositories.Interfaces
{
    /// <summary>
    /// Interface for base users repository.
    /// </summary>
    /// <typeparam name="TActions">The type of the actions.</typeparam>
    /// <typeparam name="TUsersKey">The type of the users key.</typeparam>
    public interface IBaseUsersRepository<TActions, TUsersKey>
        where TActions : class, IActionEntity
        where TUsersKey : struct
    {
        /// <summary>
        /// Determines whether the user has any action asynchronous.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="actionCodes">The action codes.</param>
        /// <returns>
        ///   <c>true</c> if the user has any of the action codes; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">actionCodes</exception>
        Task<bool> HasAnyActionAsync(TUsersKey userID, params string[] actionCodes);

        /// <summary>
        /// Lists all actions asynchronous.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>The actions list.</returns>
        Task<IEnumerable<TActions>> ListAllActionsAsync(TUsersKey userID);
    }
}