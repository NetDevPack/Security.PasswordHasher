using Jp.AspNetCore.PasswordHasher.Core;
using Jp.AspNetCore.PasswordHasher.Scrypt;
using Microsoft.AspNetCore.Identity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ScryptConfig
    {
        public static IServiceCollection UseScrypt<TUser>(this IPasswordHashBuilder builder) where TUser : class
        {

            builder.Services.Configure<ImprovedPasswordHasherOptions>(options =>
            {
                options.Strenght = builder.Options.Strenght;
                options.MemLimit = builder.Options.MemLimit;
                options.OpsLimit = builder.Options.OpsLimit;
            });
            return builder.Services.AddScoped<IPasswordHasher<TUser>, Scrypt<TUser>>();

        }
    }
}
