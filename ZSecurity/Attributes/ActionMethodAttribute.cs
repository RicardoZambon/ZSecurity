﻿using ZWebApi.Enums;

namespace ZWebApi.Attributes
{
    /// <summary>
    /// Specifies action type for allowing or denying users to access the service method.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ActionMethodAttribute : Attribute
    {
        #region Variables
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        public ActionTypes ActionType { get; }
        #endregion

        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="ZWebApi.Attributes.ActionMethodAttribute"/> class.
        /// </summary>
        /// <param name="actionType">Type of the action <see cref="Niten.System.Core.Enums.ActionTypes"/>.</param>
        public ActionMethodAttribute(ActionTypes actionType = ActionTypes.RegularUsersAndAdmins)
        {
            ActionType = actionType;
        }
        #endregion

        #region Public methods
        #endregion

        #region Private methods
        #endregion
    }
}