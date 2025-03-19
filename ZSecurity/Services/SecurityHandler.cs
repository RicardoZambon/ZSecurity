using System.Reflection;
using ZDatabase.Services.Interfaces;
using ZSecurity.Attributes;
using ZSecurity.Exceptions;
using ZSecurity.Helpers;
using ZSecurity.Interfaces;
using ZSecurity.Repositories.Interfaces;

namespace ZSecurity.Services
{
    /// <inheritdoc />
    public class SecurityHandler<TBaseUserRepository, TActions, TUsersKey> : ISecurityHandler
        where TBaseUserRepository : IBaseUsersRepository<TActions, TUsersKey>
        where TActions : class, IActionEntity
        where TUsersKey : struct
    {
        #region Constants
        private const string ADMINISTRATORS_ACTION_CODE = "AdministrativeMaster";
        #endregion

        #region Variables
        private readonly TBaseUserRepository baseUsersRepository;
        private readonly ICurrentUserProvider<TUsersKey> currentUserProvider;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the administrators action code.
        /// </summary>
        protected virtual string AdministratorsActionCode
        {
            get { return ADMINISTRATORS_ACTION_CODE; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityHandler{TBaseUserRepository, TActions, TUsersKey}"/> class.
        /// </summary>
        /// <param name="baseUsersRepository">The <see cref="IBaseUsersRepository{TActions, TUsersKey}"/> instance.</param>
        /// <param name="currentUserProvider">The <see cref="ICurrentUserProvider{TUsersKey}"/> instance.</param>
        public SecurityHandler(
                TBaseUserRepository baseUsersRepository,
                ICurrentUserProvider<TUsersKey> currentUserProvider)
        {
            this.baseUsersRepository = baseUsersRepository;
            this.currentUserProvider = currentUserProvider;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task<bool> CheckCurrentUserHasPermissionOrIsAdministratorAsync(string actionName)
        {
            if (currentUserProvider.CurrentUserID is not TUsersKey userID)
            {
                return false;
            }

            return await baseUsersRepository.HasAnyActionAsync(userID, AdministratorsActionCode, actionName);
        }

        /// <inheritdoc />
        public async Task<bool> CheckCurrentUserIsAdministratorAsync()
        {
            if (currentUserProvider.CurrentUserID is not TUsersKey userID)
            {
                return false;
            }

            return await baseUsersRepository.HasAnyActionAsync(userID, AdministratorsActionCode);
        }

        /// <inheritdoc />
        public async Task ValidateUserHasPermissionAsync()
        {
            MethodBase method = StackTraceHelper.GetStackTraceMethodImplementingAttribute<ActionMethodAttribute>()
                ?? throw new InvalidOperationException("Could not find any method implementing the ActionMethodAttribute in the stack trace.");

            string? serviceName = method.DeclaringType?.GetInterfaces().FirstOrDefault()?.Name;
            string? methodName = method.Name;

            string actionName = $"{serviceName}.{methodName}";

            if (currentUserProvider.CurrentUserID is not TUsersKey userID)
            {
                throw new MissingUserPermissionException(actionName);
            }

            IList<string> actionsToCheck = [actionName];

            ActionMethodAttribute? actionAttribute = method!.GetCustomAttribute<ActionMethodAttribute>();

            if (actionAttribute is not null)
            {
                if (actionAttribute.ActionType == Enums.ActionTypes.RegularUsersAndAdmins || actionAttribute.ActionType == Enums.ActionTypes.OnlyRegularUsers)
                {
                    actionsToCheck.Add(actionName);
                }

                if (actionAttribute.ActionType == Enums.ActionTypes.RegularUsersAndAdmins || actionAttribute.ActionType == Enums.ActionTypes.OnlyAdmins)
                {
                    actionsToCheck.Add(AdministratorsActionCode);
                }
            }

            if (!actionsToCheck.Any() || !await baseUsersRepository.HasAnyActionAsync(userID, actionsToCheck.ToArray()))
            {
                throw new MissingUserPermissionException(actionName);
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}