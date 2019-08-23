namespace Jp.AspNetCore.PasswordHasher.Core
{
    public class ImprovedPasswordHasherOptions
    {
        public long? OpsLimit { get; set; }
        public int? MemLimit { get; set; }
        public PasswordHasherStrenght Strenght { get; set; } = PasswordHasherStrenght.Sensitive;
    }
}
