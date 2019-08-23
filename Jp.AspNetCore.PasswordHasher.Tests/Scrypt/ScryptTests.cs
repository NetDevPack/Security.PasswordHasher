using Bogus;
using FluentAssertions;
using Jp.AspNetCore.PasswordHasher.Core;
using Jp.AspNetCore.PasswordHasher.Scrypt;
using Jp.AspNetCore.PasswordHasher.Tests.Fakers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using Xunit;

namespace Jp.AspNetCore.PasswordHasher.Tests.Scrypt
{
    public class ScryptTests
    {
        private readonly Faker _faker;

        public ScryptTests()
        {
            _faker = new Faker();
        }

        [Fact]
        public void ShouldBeTrueWhenPasswordStrengthSensitive()
        {
            var options = Options.Create(new ImprovedPasswordHasherOptions() { Strenght = PasswordHasherStrenght.Sensitive });

            var password = _faker.Internet.Password();
            var user = GenericUserFaker.GenerateUser().Generate();
            var scryptHasher = new Scrypt<GenericUser>(options);

            var hashedPass = scryptHasher.HashPassword(user, password);

            scryptHasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
        }

        [Fact]
        public void ShouldBeTrueWhenPasswordStrengthModerate()
        {
            var options = Options.Create(new ImprovedPasswordHasherOptions() { Strenght = PasswordHasherStrenght.Moderate });
            var password = _faker.Internet.Password();
            var user = GenericUserFaker.GenerateUser().Generate();
            var scryptHasher = new Scrypt<GenericUser>(options);

            var hashedPass = scryptHasher.HashPassword(user, password);

            scryptHasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
        }

        [Fact]
        public void ShouldBeTrueWhenPasswordStrengthInteractive()
        {
            var options = Options.Create(new ImprovedPasswordHasherOptions() { Strenght = PasswordHasherStrenght.Interactive });
            var password = _faker.Internet.Password();
            var user = GenericUserFaker.GenerateUser().Generate();
            var scryptHasher = new Scrypt<GenericUser>(options);

            var hashedPass = scryptHasher.HashPassword(user, password);

            scryptHasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
        }

        [Fact]
        public void ShouldBeTrueWhenPasswordWithCustomStrength()
        {
            var options = Options.Create(ImprovedPasswordHasherOptionsFaker.GenerateRandomOptions().Generate());
            var password = _faker.Internet.Password();
            var user = GenericUserFaker.GenerateUser().Generate();
            var scryptHasher = new Scrypt<GenericUser>(options);

            var hashedPass = scryptHasher.HashPassword(user, password);

            scryptHasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
        }
        [Fact]
        public void ShouldNotAcceptNullPasswordWhenHashingPassword()
        {
            var user = GenericUserFaker.GenerateUser().Generate();
            var scryptHasher = new Scrypt<GenericUser>();

            scryptHasher.Invoking(i => i.HashPassword(user, null))
                .Should().Throw<ArgumentNullException>();

        }

        [Fact]
        public void ShouldNotAcceptNullUserWhenHashingPassword()
        {
            var password = _faker.Internet.Password();
            var scryptHasher = new Scrypt<GenericUser>();

            scryptHasher.Invoking(i => i.HashPassword(null, password))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ShouldNotAcceptNullPasswordWhenVerifyingPassword()
        {
            var options = Options.Create(new ImprovedPasswordHasherOptions() { Strenght = PasswordHasherStrenght.Interactive });
            var password = _faker.Internet.Password();
            var user = GenericUserFaker.GenerateUser().Generate();
            var scryptHasher = new Scrypt<GenericUser>(options);

            var hashedPass = scryptHasher.HashPassword(user, password);

            scryptHasher.Invoking(i => i.VerifyHashedPassword(user, hashedPass, null))
                .Should().Throw<ArgumentNullException>();

        }


        [Fact]
        public void ShouldNotAcceptNullHashedPasswordWhenVerifyingPassword()
        {
            var options = Options.Create(new ImprovedPasswordHasherOptions() { Strenght = PasswordHasherStrenght.Interactive });
            var password = _faker.Internet.Password();
            var user = GenericUserFaker.GenerateUser().Generate();
            var scryptHasher = new Scrypt<GenericUser>(options);


            scryptHasher.Invoking(i => i.VerifyHashedPassword(user, null, password))
                .Should().Throw<ArgumentNullException>();

        }


        [Fact]
        public void ShouldNotAcceptNullUserWhenVerifyingPassword()
        {
            var options = Options.Create(new ImprovedPasswordHasherOptions() { Strenght = PasswordHasherStrenght.Interactive });
            var password = _faker.Internet.Password();
            var user = GenericUserFaker.GenerateUser().Generate();
            var scryptHasher = new Scrypt<GenericUser>(options);

            var hashedPass = scryptHasher.HashPassword(user, password);

            scryptHasher.Invoking(i => i.VerifyHashedPassword(null, hashedPass, password))
                .Should().Throw<ArgumentNullException>();

        }
    }
}
