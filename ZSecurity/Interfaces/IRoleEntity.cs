namespace ZWebApi.Interfaces
{
    /// <summary>
    /// Interface for role entities.
    /// </summary>
    public interface IRoleEntity<TActions>
        where TActions : class, IActionEntity
    {
        /// <summary>
        /// Gets or sets the actions.
        /// </summary>
        /// <value>
        /// The actions.
        /// </value>
        ICollection<TActions>? Actions { get; set; }
    }
}