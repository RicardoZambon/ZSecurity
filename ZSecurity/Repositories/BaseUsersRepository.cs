using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDatabase.Interfaces;
using ZDatabase.Services.Interfaces;
using ZSecurity.Interfaces;

namespace ZSecurity.Repositories
{
    public abstract class BaseUsersRepository<TActions, TUsersKey>
        where TActions : class, IActionEntity
        where TUsersKey : struct
    {
        #region Variables
        private readonly IDbContext dbContext;
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
        /// <summary>
        /// Determines whether the user has any action asynchronous.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="actionCodes">The action codes.</param>
        /// <returns>
        ///   <c>true</c> if the user has any of the action codes; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">actionCodes</exception>
        public async Task<bool> HasAnyActionAsync(TUsersKey userID, params string[] actionCodes)
        {
            if (!actionCodes.Any())
            {
                throw new ArgumentNullException(nameof(actionCodes));
            }

            return (await ListAllActionsAsync(userID))
                .Any(x => actionCodes.Contains(x.Code));
        }

        /// <summary>
        /// Lists all actions asynchronous.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>The actions list.</returns>
        public abstract Task<IEnumerable<TActions>> ListAllActionsAsync(TUsersKey userID);
        #endregion

        #region Private methods
        #endregion
    }
}
