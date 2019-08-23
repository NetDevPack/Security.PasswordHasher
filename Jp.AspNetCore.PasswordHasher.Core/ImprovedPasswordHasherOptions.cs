namespace Jp.AspNetCore.PasswordHasher.Core
{
    public class ImprovedPasswordHasherOptions
    {
        /// <summary>
        /// opslimit represents a maximum amount of computations to perform.
        /// Raising this number will make the function require more CPU cycles to compute a key.
        /// This number must be between
        /// </summary>
        public long? OpsLimit { get; set; }

        /// <summary>
        /// memlimit is the maximum amount of RAM that the function will use, in bytes.
        /// </summary>
        public int? MemLimit { get; set; }

        /// <summary>
        /// Password Strengten. If set will change values from OpsLimit and MemLimit
        /// </summary>
        public PasswordHasherStrenght Strenght { get; set; } = PasswordHasherStrenght.Sensitive;

        /// <summary>
        /// The log2 of the number of rounds of hashing to apply on BCrypt.
        /// The work factor therefore increases as 2**workFactor.
        /// </summary>
        public int WorkFactor { get; set; } = 10;

        /// <summary>
        /// The log2 of the number of rounds of hashing to apply on BCrypt.
        /// The work factor therefore increases as 2**workFactor.
        /// </summary>
        public BcryptSaltRevision SaltRevision { get; set; } = BcryptSaltRevision.Revision2B;
    }
}
