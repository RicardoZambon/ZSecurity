using ZDatabase.Interfaces;
using ZSecurity.Repositories;
using ZSecurity.UnitTests.Fakes.Entities;

namespace ZSecurity.UnitTests.Fakes.Repositories
{
    internal sealed class UsersRepositoryFake : BaseUsersRepository<ActionEntityFake, long>
    {
        private readonly IEnumerable<ActionEntityFake> actions;

        internal UsersRepositoryFake(IDbContext dbContext, params string?[] actionCodes)
            : base(dbContext)
        {
            actions = actionCodes.Select(x => new ActionEntityFake { Code = x }).ToArray();
        }

        /// <summary>
        /// Exposes the protected <c>dbContext</c> field so the base constructor can be asserted.
        /// </summary>
        internal IDbContext ExposedDbContext
        {
            get { return dbContext; }
        }

        /// <summary>
        /// The user identifier the last <see cref="ListAllActionsAsync(long)"/> call received.
        /// </summary>
        internal long? LastRequestedUserID { get; private set; }

        public override Task<IEnumerable<ActionEntityFake>> ListAllActionsAsync(long userID)
        {
            LastRequestedUserID = userID;
            return Task.FromResult(actions);
        }
    }
}
