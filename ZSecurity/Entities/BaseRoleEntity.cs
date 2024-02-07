using ZDatabase.Entities;
using ZDatabase.Security.Interfaces;

namespace ZWebApi.Entities
{
    /// <summary>
    /// Abstract class for role entities.
    /// </summary>
    /// <typeparam name="TUsers">The type of the users.</typeparam>
    /// <typeparam name="TUsersKey">The type of the users key.</typeparam>
    /// <seealso cref="ZDatabase.Entities.AuditableEntity&lt;TUsers, TUsersKey&gt;" />
    /// <seealso cref="ZDatabase.Security.Interfaces.IActionEntity" />
    public class BaseRoleEntity<TUsers, TActions, TUsersKey> : AuditableEntity<TUsers, TUsersKey>, IRoleEntity<TActions>
        where TUsers : class
        where TActions : class, IActionEntity
        where TUsersKey : struct
    {
        /// <inheritdoc />
        public ICollection<TActions>? Actions { get; set; }
    }
}