namespace ZSecurity.UnitTests.Fakes.Services
{
    internal interface ISecuredServiceFake
    {
        Task DefaultActionAsync();

        Task OnlyAdminsActionAsync();

        Task OnlyRegularUsersActionAsync();

        Task UnattributedActionAsync();

        Task NestedCallActionAsync();
    }
}
