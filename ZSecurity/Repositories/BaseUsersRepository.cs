using ZDatabase.Interfaces;
using ZSecurity.Interfaces;
using ZSecurity.Repositories.Interfaces;

namespace ZSecurity.Repositories
{
    /// <inheritdoc />
    public abstract class BaseUsersRepository<TActions, TUsersKey> : IBaseUsersRepository<TActions, TUsersKey>
        where TActions : class, IActionEntity
        where TUsersKey : struct
    {
        #region Variables
        protected readonly IDbContext dbContext;
        #endregion

        #region Properties
        #endregion

        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityHandler{TUsersKey}"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="ZDatabase.Interfaces.IDbContext"/> instance.</param>
        public BaseUsersRepository(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Public methods        
        /// <inheritdoc />
        public async Task<bool> HasAnyActionAsync(TUsersKey userID, params string[] actionCodes)
        {
            if (!actionCodes.Any())
            {
                throw new ArgumentNullException(nameof(actionCodes));
            }

            return (await ListAllActionsAsync(userID))
                .Any(x => actionCodes.Contains(x.Code));
        }

        /// <inheritdoc />
        public abstract Task<IEnumerable<TActions>> ListAllActionsAsync(TUsersKey userID);
        #endregion

        #region Private methods
        #endregion
    }
}