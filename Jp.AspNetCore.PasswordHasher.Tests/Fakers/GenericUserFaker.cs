using Bogus;

namespace Jp.AspNetCore.PasswordHasher.Tests.Fakers
{
    public class GenericUserFaker
    {
        public static Faker<GenericUser> GenerateUser()
        {
            return new Faker<GenericUser>();
        }
    }
}
