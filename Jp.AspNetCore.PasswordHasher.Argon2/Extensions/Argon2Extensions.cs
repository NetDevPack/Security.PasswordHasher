using Jp.AspNetCore.PasswordHasher.Argon2;
using Microsoft.AspNetCore.Identity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Argon2IdConfig
    {
        public static IServiceCollection UseScrypt<TUser>(this IPasswordHashBuilder builder) where TUser : class
        {
            return builder.Services.AddScoped<IPasswordHasher<TUser>, Argon2Id<TUser>>();
        }
    }
}
