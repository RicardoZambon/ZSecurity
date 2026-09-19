using System.Runtime.CompilerServices;

// NSubstitute builds its substitutes with Castle DynamicProxy, which emits them into the
// DynamicProxyGenAssembly2 dynamic assembly. Substituting a generic interface closed over an
// internal fake type (for example IBaseUsersRepository<ActionEntityFake, long>) therefore
// requires that assembly to see the internals of this one.
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
