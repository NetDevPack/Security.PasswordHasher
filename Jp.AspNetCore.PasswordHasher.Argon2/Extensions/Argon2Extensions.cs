using Jp.AspNetCore.PasswordHasher.Argon2;
using Jp.AspNetCore.PasswordHasher.Core;
using Microsoft.AspNetCore.Identity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Argon2IdConfig
    {
        public static IServiceCollection UseArgon2<TUser>(this IPasswordHashBuilder builder) where TUser : class
        {
            builder.Services.Configure<ImprovedPasswordHasherOptions>(options =>
            {
                options.Strenght = builder.Options.Strenght;
                options.MemLimit = builder.Options.MemLimit;
                options.OpsLimit = builder.Options.OpsLimit;
                options.WorkFactor = builder.Options.WorkFactor;
            });
            return builder.Services.AddScoped<IPasswordHasher<TUser>, Argon2Id<TUser>>();
        }
    }
}
