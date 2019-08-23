using Microsoft.Extensions.DependencyInjection;
using System;

namespace Jp.AspNetCore.PasswordHasher.Core.DependencyInjection
{
    /// <summary>
    /// Helper for DI configuration
    /// </summary>
    public class PasswordHasherBuilder : IPasswordHashBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordHasherBuilder"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <exception cref="System.ArgumentNullException">services</exception>
        public PasswordHasherBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Options = new ImprovedPasswordHasherOptions();
        }

        public ImprovedPasswordHasherOptions Options { get; }

        public IServiceCollection Services { get; }
        public IPasswordHashBuilder WithMemLimit(int memLimit)
        {
            Options.MemLimit = memLimit;
            return this;
        }

        public IPasswordHashBuilder WithOpsLimit(long opsLimit)
        {
            Options.OpsLimit = opsLimit;
            return this;
        }

        public IPasswordHashBuilder WithStrenghten(PasswordHasherStrenght strenght)
        {
            Options.Strenght = strenght;
            return this;
        }
    }
}
