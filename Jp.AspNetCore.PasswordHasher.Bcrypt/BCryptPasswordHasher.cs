using BCrypt.Net;
using Jp.AspNetCore.PasswordHasher.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;

namespace Jp.AspNetCore.PasswordHasher.Bcrypt
{
    public class BCryptPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        private readonly ImprovedPasswordHasherOptions _options;

        /// <summary>
        /// Creates a new instance of <see cref="PasswordHasher{TUser}"/>.
        /// </summary>
        /// <param name="optionsAccessor">The options for this instance.</param>
        public BCryptPasswordHasher(IOptions<ImprovedPasswordHasherOptions> optionsAccessor = null)
        {
            _options = optionsAccessor?.Value ?? new ImprovedPasswordHasherOptions();
        }

        public string HashPassword(TUser user, string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), $"{nameof(password)} should not be null");
            if (user == null)
                throw new ArgumentNullException(nameof(user), $"{nameof(user)} should not be null");

            return BCrypt.Net.BCrypt.HashPassword(password, _options.WorkFactor, GetSaltRevision());
        }

        /// <summary>
        /// From BcryptSaltRevision to SaltRevision
        /// </summary>
        /// <returns></returns>
        private SaltRevision GetSaltRevision()
        {
            switch (_options.SaltRevision)
            {
                case BcryptSaltRevision.Revision2:
                    return SaltRevision.Revision2;
                case BcryptSaltRevision.Revision2A:
                    return SaltRevision.Revision2A;
                case BcryptSaltRevision.Revision2B:
                    return SaltRevision.Revision2B;
                case BcryptSaltRevision.Revision2X:
                    return SaltRevision.Revision2X;
                case BcryptSaltRevision.Revision2Y:
                    return SaltRevision.Revision2Y;
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

            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword)
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed;
        }
    }
}
