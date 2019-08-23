using System;
using Jp.AspNetCore.PasswordHasher.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sodium;

namespace Jp.AspNetCore.PasswordHasher.Argon2
{
    public class Argon2Id<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        private readonly ImprovedPasswordHasherOptions _options;

        /// <summary>
        /// Creates a new instance of <see cref="PasswordHasher{TUser}"/>.
        /// </summary>
        /// <param name="optionsAccessor">The options for this instance.</param>
        public Argon2Id(IOptions<ImprovedPasswordHasherOptions> optionsAccessor = null)
        {
            _options = optionsAccessor?.Value ?? new ImprovedPasswordHasherOptions();

        }
        public string HashPassword(TUser user, string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), $"{nameof(password)} should not be null");
            if (user == null)
                throw new ArgumentNullException(nameof(user), $"{nameof(user)} should not be null");

            if (_options.OpsLimit.HasValue && _options.MemLimit.HasValue)
                return PasswordHash.ArgonHashString(password, _options.OpsLimit.Value, _options.MemLimit.Value);

            switch (_options.Strenght)
            {
                case PasswordHasherStrenght.Interactive:
                    return PasswordHash.ArgonHashString(password);
                case PasswordHasherStrenght.Moderate:
                    return PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Moderate);
                case PasswordHasherStrenght.Sensitive:
                    return PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Sensitive);
                default:
                    throw new ArgumentOutOfRangeException();
            }


        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), $"{nameof(user)} should not be null");

            if (string.IsNullOrEmpty(hashedPassword))
                throw new ArgumentNullException(nameof(hashedPassword), $"{nameof(hashedPassword)} should not be null");

            if (string.IsNullOrEmpty(providedPassword))
                throw new ArgumentNullException(nameof(providedPassword), $"{nameof(providedPassword)} should not be null");

            return PasswordHash.ArgonHashStringVerify(hashedPassword, providedPassword)
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed;
        }
    }
}
