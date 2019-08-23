using Jp.AspNetCore.PasswordHasher.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Jp.AspNetCore.PasswordHasher.Bcrypt.Extensions
{
    public static class BCryptConfig
    {
        /// <summary>
        /// Use Scrypt password hashing algorithm.
        /// </summary>
        public static IServiceCollection UseScrypt<TUser>(this IPasswordHashBuilder builder) where TUser : class
        {
            builder.Services.Configure<ImprovedPasswordHasherOptions>(options =>
            {
                options.Strenght = builder.Options.Strenght;
                options.MemLimit = builder.Options.MemLimit;
                options.OpsLimit = builder.Options.OpsLimit;
                options.WorkFactor = builder.Options.WorkFactor;
            });
            return builder.Services.AddScoped<IPasswordHasher<TUser>, BCryptPasswordHasher<TUser>>();
        }
    }
}
