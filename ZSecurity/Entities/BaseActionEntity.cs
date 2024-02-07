using ZDatabase.Entities;
using ZDatabase.Security.Interfaces;

namespace ZWebApi.Entities
{
    /// <summary>
    /// Abstract class for action entities.
    /// </summary>
    /// <typeparam name="TUsers">The type of the users.</typeparam>
    /// <typeparam name="TUsersKey">The type of the users key.</typeparam>
    /// <seealso cref="ZDatabase.Entities.AuditableEntity&lt;TUsers, TUsersKey&gt;" />
    /// <seealso cref="ZDatabase.Security.Interfaces.IActionEntity" />
    public class BaseActionEntity<TUsers, TUsersKey> : AuditableEntity<TUsers, TUsersKey>, IActionEntity
        where TUsers : class
        where TUsersKey : struct
    {
        /// <inheritdoc />
        public string? Code { get; set; }
    }
}