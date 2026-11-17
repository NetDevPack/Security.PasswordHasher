using Bogus;
using AwesomeAssertions;
using NetDevPack.Security.PasswordHasher.Argon2;
using NetDevPack.Security.PasswordHasher.Core;
using NetDevPack.Security.PasswordHasher.Tests.Fakers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Xunit;

namespace NetDevPack.Security.PasswordHasher.Tests.Argon2;

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
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(new ImprovedPasswordHasherOptions() { Strength = PasswordHasherStrength.Sensitive });

        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var argon2Hasher = new Argon2Id<GenericUser>(passwordHasher, options);

        var hashedPass = argon2Hasher.HashPassword(user, password);

        argon2Hasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
    }

    [Fact]
    public void ShouldBeTrueWhenPasswordStrengthModerate()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(new ImprovedPasswordHasherOptions() { Strength = PasswordHasherStrength.Moderate });
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var argon2Hasher = new Argon2Id<GenericUser>(passwordHasher, options);

        var hashedPass = argon2Hasher.HashPassword(user, password);

        argon2Hasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
    }

    [Fact]
    public void ShouldBeTrueWhenPasswordStrengthInteractive()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(new ImprovedPasswordHasherOptions() { Strength = PasswordHasherStrength.Interactive });
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var argon2Hasher = new Argon2Id<GenericUser>(passwordHasher, options);

        var hashedPass = argon2Hasher.HashPassword(user, password);

        argon2Hasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
    }

    [Fact]
    public void ShouldBeTrueWhenPasswordWithCustomStrength()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(ImprovedPasswordHasherOptionsFaker.GenerateRandomOptions().Generate());
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var argon2Hasher = new Argon2Id<GenericUser>(passwordHasher, options);

        var hashedPass = argon2Hasher.HashPassword(user, password);

        argon2Hasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
    }
    [Fact]
    public void ShouldNotAcceptNullPasswordWhenHashingPassword()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var user = GenericUserFaker.GenerateUser().Generate();
        var argon2Hasher = new Argon2Id<GenericUser>(passwordHasher);

        argon2Hasher.Invoking(i => i.HashPassword(user, null))
            .Should().Throw<ArgumentNullException>();

    }

    [Fact]
    public void ShouldNotAcceptNullUserWhenHashingPassword()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var password = _faker.Internet.Password();
        var argon2Hasher = new Argon2Id<GenericUser>(passwordHasher);

        argon2Hasher.Invoking(i => i.HashPassword(null, password))
            .Should().Throw<ArgumentNullException>();

    }

    [Fact]
    public void ShouldNotAcceptNullPasswordWhenVerifyingPassword()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(new ImprovedPasswordHasherOptions() { Strength = PasswordHasherStrength.Interactive });
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var argon2Hasher = new Argon2Id<GenericUser>(passwordHasher, options);

        var hashedPass = argon2Hasher.HashPassword(user, password);

        argon2Hasher.Invoking(i => i.VerifyHashedPassword(user, hashedPass, null))
            .Should().Throw<ArgumentNullException>();

    }


    [Fact]
    public void ShouldNotAcceptNullHashedPasswordWhenVerifyingPassword()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(new ImprovedPasswordHasherOptions() { Strength = PasswordHasherStrength.Interactive });
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var argon2Hasher = new Argon2Id<GenericUser>(passwordHasher, options);


        argon2Hasher.Invoking(i => i.VerifyHashedPassword(user, null, password))
            .Should().Throw<ArgumentNullException>();

    }


    [Fact]
    public void ShouldNotAcceptNullUserWhenVerifyingPassword()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(new ImprovedPasswordHasherOptions() { Strength = PasswordHasherStrength.Interactive });
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var argon2Hasher = new Argon2Id<GenericUser>(passwordHasher, options);

        var hashedPass = argon2Hasher.HashPassword(user, password);

        argon2Hasher.Invoking(i => i.VerifyHashedPassword(null, hashedPass, password))
            .Should().Throw<ArgumentNullException>();

    }


    [Fact]
    public void ShouldMemLimitSameOfConfiguration()
    {
        var memLimit = _faker.Random.Int(1024, 1073741824);
        var services = new ServiceCollection();
        services.UpgradePasswordSecurity().WithMemLimit(memLimit).UseArgon2<GenericUser>();

        var provider = services.BuildServiceProvider();
        var passwordHasherOptions = (IOptions<ImprovedPasswordHasherOptions>)provider.GetService(typeof(IOptions<ImprovedPasswordHasherOptions>));

        passwordHasherOptions.Value.MemLimit.Should().Be(memLimit);
    }

    [Fact]
    public void ShouldOpsLimitSameOfConfiguration()
    {
        var opsLimit = _faker.Random.Long(3L, 16L);
        var services = new ServiceCollection();
        services.UpgradePasswordSecurity().WithOpsLimit(opsLimit).UseArgon2<GenericUser>();

        var provider = services.BuildServiceProvider();
        var passwordHasherOptions = (IOptions<ImprovedPasswordHasherOptions>)provider.GetService(typeof(IOptions<ImprovedPasswordHasherOptions>));

        passwordHasherOptions.Value.OpsLimit.Should().Be(opsLimit);
    }

    [Theory]
    [InlineData(PasswordHasherStrength.Moderate)]
    [InlineData(PasswordHasherStrength.Sensitive)]
    [InlineData(PasswordHasherStrength.Interactive)]
    public void ShouldPasswordStrengthSameOfConfiguration(PasswordHasherStrength strength)
    {
        var services = new ServiceCollection();
        services.UpgradePasswordSecurity().WithStrengthen(strength).UseArgon2<GenericUser>();

        var provider = services.BuildServiceProvider();
        var passwordHasherOptions = (IOptions<ImprovedPasswordHasherOptions>)provider.GetService(typeof(IOptions<ImprovedPasswordHasherOptions>));

        passwordHasherOptions.Value.Strength.Should().Be(strength);
    }


    [Fact]
    public void ShouldConfigurationUseArgon2()
    {
        var services = new ServiceCollection();
        services.UpgradePasswordSecurity().UseArgon2<GenericUser>();

        var provider = services.BuildServiceProvider();
        var passwordHasher = (IPasswordHasher<GenericUser>)provider.GetService(typeof(IPasswordHasher<GenericUser>));

        passwordHasher.Should().BeOfType<Argon2Id<GenericUser>>();
    }
}