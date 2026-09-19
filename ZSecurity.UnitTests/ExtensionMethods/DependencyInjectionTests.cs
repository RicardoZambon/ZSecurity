using Microsoft.Extensions.DependencyInjection;
using ZSecurity.ExtensionMethods;
using ZSecurity.Repositories.Interfaces;
using ZSecurity.Services;
using ZSecurity.UnitTests.Factories;
using ZSecurity.UnitTests.Fakes.Entities;
using ZSecurity.UnitTests.Fakes.Services;
using ZDatabase.Services.Interfaces;

namespace ZSecurity.UnitTests.ExtensionMethods
{
    /// <summary>
    /// Unit tests for <see cref="ZSecurity.ExtensionMethods.DependencyInjection"/>.
    /// </summary>
    public class DependencyInjectionTests
    {
        /// <summary>
        /// Test the AddSecurityHandlerService should register the handler as the scoped implementation of the interface.
        /// </summary>
        [Fact]
        public void AddSecurityHandlerService_Pass_RegistersTheHandlerAsScoped()
        {
            // Arrange
            ServiceCollection services = new();

            // Act
            services.AddSecurityHandlerService<SecurityHandlerFake>();

            // Assert
            services.Should().ContainSingle();

            ServiceDescriptor descriptor = services.Single();
            descriptor.ServiceType.Should().Be<ISecurityHandler>();
            descriptor.ImplementationType.Should().Be<SecurityHandlerFake>();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Test the AddSecurityHandlerService should return the same service collection it received.
        /// </summary>
        [Fact]
        public void AddSecurityHandlerService_Pass_ReturnsTheSameServiceCollection()
        {
            // Arrange
            ServiceCollection services = new();

            // Act
            IServiceCollection result = services.AddSecurityHandlerService<SecurityHandlerFake>();

            // Assert
            result.Should().BeSameAs(services);
        }

        /// <summary>
        /// Test the AddSecurityHandlerService should register a handler that can actually be resolved.
        /// </summary>
        [Fact]
        public void AddSecurityHandlerService_Pass_RegistersAResolvableHandler()
        {
            // Arrange
            ServiceCollection services = new();
            services.AddSingleton(SecurityHandlerFactory.CreateRepository(true));
            services.AddSingleton(SecurityHandlerFactory.CreateCurrentUserProvider(SecurityHandlerFactory.UserID));

            // Act
            services.AddSecurityHandlerService<SecurityHandlerFake>();

            // Assert
            using ServiceProvider provider = services.BuildServiceProvider();
            using IServiceScope scope = provider.CreateScope();

            ISecurityHandler handler = scope.ServiceProvider.GetRequiredService<ISecurityHandler>();
            handler.Should().BeOfType<SecurityHandlerFake>();
        }

        /// <summary>
        /// Test the AddSecurityHandlerService should give every scope its own handler instance.
        /// </summary>
        [Fact]
        public void AddSecurityHandlerService_Pass_ResolvesOneHandlerPerScope()
        {
            // Arrange
            ServiceCollection services = new();
            services.AddSingleton(SecurityHandlerFactory.CreateRepository(true));
            services.AddSingleton(SecurityHandlerFactory.CreateCurrentUserProvider(SecurityHandlerFactory.UserID));
            services.AddSecurityHandlerService<SecurityHandlerFake>();

            using ServiceProvider provider = services.BuildServiceProvider();

            // Act
            using IServiceScope firstScope = provider.CreateScope();
            using IServiceScope secondScope = provider.CreateScope();

            ISecurityHandler firstHandler = firstScope.ServiceProvider.GetRequiredService<ISecurityHandler>();
            ISecurityHandler secondHandler = secondScope.ServiceProvider.GetRequiredService<ISecurityHandler>();

            // Assert
            firstScope.ServiceProvider.GetRequiredService<ISecurityHandler>().Should().BeSameAs(firstHandler);
            secondHandler.Should().NotBeSameAs(firstHandler);
        }

        /// <summary>
        /// Test the AddSecurityHandlerService should keep registrations already made in the collection.
        /// </summary>
        [Fact]
        public void AddSecurityHandlerService_Pass_KeepsExistingRegistrations()
        {
            // Arrange
            ServiceCollection services = new();
            services.AddSingleton(Substitute.For<IBaseUsersRepository<ActionEntityFake, long>>());
            services.AddSingleton(Substitute.For<ICurrentUserProvider<long>>());

            // Act
            services.AddSecurityHandlerService<SecurityHandlerFake>();

            // Assert
            services.Should().HaveCount(3);
            services.Should().Contain(x => x.ServiceType == typeof(ISecurityHandler));
        }
    }
}
