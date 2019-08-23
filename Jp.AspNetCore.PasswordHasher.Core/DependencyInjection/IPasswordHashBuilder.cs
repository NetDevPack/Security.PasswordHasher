using Jp.AspNetCore.PasswordHasher.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public interface IPasswordHashBuilder
    {
        /// <summary>Gets the services.</summary>
        /// <value>The services.</value>
        IServiceCollection Services { get; }
        ImprovedPasswordHasherOptions Options { get; }
        IPasswordHashBuilder WithMemLimit(int memLimit);

        IPasswordHashBuilder WithOpsLimit(long opsLimit);

        IPasswordHashBuilder WithStrenghten(PasswordHasherStrenght strenght);

    }
}