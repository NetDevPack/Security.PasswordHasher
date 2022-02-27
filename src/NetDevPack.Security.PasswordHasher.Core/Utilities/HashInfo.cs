namespace NetDevPack.Security.PasswordHasher.Core.Utilities
{
    public class HashInfo
    {
        public HashInfo(string hash)
        {

            IsArgon2 = CheckArgon2(hash);
            if (IsArgon2) return;

            IsBcrypt = CheckBcrypt(hash);
            if (IsBcrypt) return;

            IsScrypt = CheckScrypt(hash);
            if (IsScrypt) return;

            try
            {
                IsAspNetV2 = CheckAspnetV2(hash);
                if (IsAspNetV2) return;

                IsAspNetV3 = CheckAspnetV3(hash);
            }
            catch
            {
                IsAspNetV2 = false;
                IsAspNetV3 = false;
            }
        }

        public bool IsAspNetV2 { get; }
        public bool IsAspNetV3 { get; }
        public bool IsScrypt { get; }

        public bool IsBcrypt { get; }

        public bool IsArgon2 { get; }

        private static bool CheckAspnetV2(string hash) => hash.FromBase64().ToPlainHexDumpStyle().StartsWith("00");
        private static bool CheckAspnetV3(string hash) => hash.FromBase64().ToPlainHexDumpStyle().StartsWith("01");
        private static bool CheckBcrypt(string hash) => hash.Contains("$2b$") || hash.Contains("$2a$");
        private static bool CheckArgon2(string hash) => hash.Contains("$argon2id$");
        private static bool CheckScrypt(string hash) => hash.Contains("$s2$");

    }
}