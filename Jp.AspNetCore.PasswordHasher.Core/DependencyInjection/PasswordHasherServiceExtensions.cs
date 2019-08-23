using Jp.AspNetCore.PasswordHasher.Core.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PasswordHasherServiceExtensions
    {
        /// <summary>
        /// Creates a builder.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IPasswordHashBuilder UseCustomHashPasswordBuilder(this IServiceCollection services)
        {
            return new PasswordHasherBuilder(services);
        }

        public static IPasswordHashBuilder UpgradePasswordSecurity(this IServiceCollection services)
        {
            return services.UseCustomHashPasswordBuilder();
        }


    }
}
