
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using Jp.AspNetCore.PasswordHasher.Argon2;
using Jp.AspNetCore.PasswordHasher.Core;
using Jp.AspNetCore.PasswordHasher.Tests.Fakers;
using Xunit;

namespace Jp.AspNetCore.PasswordHasher.Tests.Argon2
{
    public class Argon2Tests
    {
        private readonly Faker _faker;

        public Argon2Tests()
        {
            _faker = new Faker();
        }

        [Fact]
        public void ShouldBeTrueWhenPasswordStrengthSensitive()
        {
            var options = Options.Create(new ImprovedPasswordHasherOptions() { Strenght = PasswordHasherStrenght.Sensitive });

            var password = _faker.Internet.Password();
            var user = GenericUserFaker.GenerateUser().Generate();
            var argon2Hasher = new Argon2Id<GenericUser>(options);

            var hashedPass = argon2Hasher.HashPassword(user, password);

            argon2Hasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
        }

        [Fact]
        public void ShouldBeTrueWhenPasswordStrengthModerate()
        {
            var options = Options.Create(new ImprovedPasswordHasherOptions() { Strenght = PasswordHasherStrenght.Moderate });
            var password = _faker.Internet.Password();
            var user = GenericUserFaker.GenerateUser().Generate();
            var argon2Hasher = new Argon2Id<GenericUser>(options);

            var hashedPass = argon2Hasher.HashPassword(user, password);

            argon2Hasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
        }

        [Fact]
        public void ShouldBeTrueWhenPasswordStrengthInteractive()
        {
            var options = Options.Create(new ImprovedPasswordHasherOptions() { Strenght = PasswordHasherStrenght.Interactive });
            var password = _faker.Internet.Password();
            var user = GenericUserFaker.GenerateUser().Generate();
            var argon2Hasher = new Argon2Id<GenericUser>(options);

            var hashedPass = argon2Hasher.HashPassword(user, password);

            argon2Hasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
        }

        [Fact]
        public void ShouldBeTrueWhenPasswordWithCustomStrength()
        {
            var options = Options.Create(ImprovedPasswordHasherOptionsFaker.GenerateRandomOptions().Generate());
            var password = _faker.Internet.Password();
            var user = GenericUserFaker.GenerateUser().Generate();
            var argon2Hasher = new Argon2Id<GenericUser>(options);

            var hashedPass = argon2Hasher.HashPassword(user, password);

            argon2Hasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
        }
        [Fact]
        public void ShouldNotAcceptNullPasswordWhenHashingPassword()
        {
            var user = GenericUserFaker.GenerateUser().Generate();
            var argon2Hasher = new Argon2Id<GenericUser>();

            argon2Hasher.Invoking(i => i.HashPassword(user, null))
                .Should().Throw<ArgumentNullException>();

        }

        [Fact]
        public void ShouldNotAcceptNullUserWhenHashingPassword()
        {
            var password = _faker.Internet.Password();
            var argon2Hasher = new Argon2Id<GenericUser>();

            argon2Hasher.Invoking(i => i.HashPassword(null, password))
                .Should().Throw<ArgumentNullException>();

        }

        [Fact]
        public void ShouldNotAcceptNullPasswordWhenVerifyingPassword()
        {
            var options = Options.Create(new ImprovedPasswordHasherOptions() { Strenght = PasswordHasherStrenght.Interactive });
            var password = _faker.Internet.Password();
            var user = GenericUserFaker.GenerateUser().Generate();
            var argon2Hasher = new Argon2Id<GenericUser>(options);

            var hashedPass = argon2Hasher.HashPassword(user, password);

            argon2Hasher.Invoking(i => i.VerifyHashedPassword(user, hashedPass, null))
                .Should().Throw<ArgumentNullException>();

        }


        [Fact]
        public void ShouldNotAcceptNullHashedPasswordWhenVerifyingPassword()
        {
            var options = Options.Create(new ImprovedPasswordHasherOptions() { Strenght = PasswordHasherStrenght.Interactive });
            var password = _faker.Internet.Password();
            var user = GenericUserFaker.GenerateUser().Generate();
            var argon2Hasher = new Argon2Id<GenericUser>(options);


            argon2Hasher.Invoking(i => i.VerifyHashedPassword(user, null, password))
                .Should().Throw<ArgumentNullException>();

        }


        [Fact]
        public void ShouldNotAcceptNullUserWhenVerifyingPassword()
        {
            var options = Options.Create(new ImprovedPasswordHasherOptions() { Strenght = PasswordHasherStrenght.Interactive });
            var password = _faker.Internet.Password();
            var user = GenericUserFaker.GenerateUser().Generate();
            var argon2Hasher = new Argon2Id<GenericUser>(options);

            var hashedPass = argon2Hasher.HashPassword(user, password);

            argon2Hasher.Invoking(i => i.VerifyHashedPassword(null, hashedPass, password))
                .Should().Throw<ArgumentNullException>();

        }
    }
}
