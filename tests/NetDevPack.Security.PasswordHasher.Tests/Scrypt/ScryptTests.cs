using Bogus;
using AwesomeAssertions;
using NetDevPack.Security.PasswordHasher.Core;
using NetDevPack.Security.PasswordHasher.Scrypt;
using NetDevPack.Security.PasswordHasher.Tests.Fakers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Xunit;

namespace NetDevPack.Security.PasswordHasher.Tests.Scrypt;

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
        var options = Options.Create(new ImprovedPasswordHasherOptions() { Strength = PasswordHasherStrength.Sensitive });
        var passwordHasher = new PasswordHasher<GenericUser>();
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new Scrypt<GenericUser>(passwordHasher, options);

        var hashedPass = scryptHasher.HashPassword(user, password);

        scryptHasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
    }

    [Fact]
    public void ShouldBeTrueWhenPasswordStrengthModerate()
    {
        var options = Options.Create(new ImprovedPasswordHasherOptions() { Strength = PasswordHasherStrength.Moderate });
        var passwordHasher = new PasswordHasher<GenericUser>();
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new Scrypt<GenericUser>(passwordHasher, options);

        var hashedPass = scryptHasher.HashPassword(user, password);

        scryptHasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
    }

    [Fact]
    public void ShouldBeTrueWhenPasswordStrengthInteractive()
    {
        var options = Options.Create(new ImprovedPasswordHasherOptions() { Strength = PasswordHasherStrength.Interactive });
        var passwordHasher = new PasswordHasher<GenericUser>();
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new Scrypt<GenericUser>(passwordHasher, options);

        var hashedPass = scryptHasher.HashPassword(user, password);

        scryptHasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
    }

    [Fact]
    public void ShouldBeTrueWhenPasswordWithCustomStrength()
    {
        var options = Options.Create(ImprovedPasswordHasherOptionsFaker.GenerateRandomOptions().Generate());
        var passwordHasher = new PasswordHasher<GenericUser>();
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new Scrypt<GenericUser>(passwordHasher, options);

        var hashedPass = scryptHasher.HashPassword(user, password);

        scryptHasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
    }
    [Fact]
    public void ShouldNotAcceptNullPasswordWhenHashingPassword()
    {
        var user = GenericUserFaker.GenerateUser().Generate();
        var passwordHasher = new PasswordHasher<GenericUser>();
        var scryptHasher = new Scrypt<GenericUser>(passwordHasher);

        scryptHasher.Invoking(i => i.HashPassword(user, null))
            .Should().Throw<ArgumentNullException>();

    }

    [Fact]
    public void ShouldNotAcceptNullUserWhenHashingPassword()
    {
        var password = _faker.Internet.Password();
        var passwordHasher = new PasswordHasher<GenericUser>();
        var scryptHasher = new Scrypt<GenericUser>(passwordHasher);

        scryptHasher.Invoking(i => i.HashPassword(null, password))
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ShouldNotAcceptNullPasswordWhenVerifyingPassword()
    {
        var options = Options.Create(new ImprovedPasswordHasherOptions() { Strength = PasswordHasherStrength.Interactive });
        var passwordHasher = new PasswordHasher<GenericUser>();
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new Scrypt<GenericUser>(passwordHasher, options);

        var hashedPass = scryptHasher.HashPassword(user, password);

        scryptHasher.Invoking(i => i.VerifyHashedPassword(user, hashedPass, null))
            .Should().Throw<ArgumentNullException>();

    }


    [Fact]
    public void ShouldNotAcceptNullHashedPasswordWhenVerifyingPassword()
    {
        var options = Options.Create(new ImprovedPasswordHasherOptions() { Strength = PasswordHasherStrength.Interactive });
        var passwordHasher = new PasswordHasher<GenericUser>();
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new Scrypt<GenericUser>(passwordHasher, options);


        scryptHasher.Invoking(i => i.VerifyHashedPassword(user, null, password))
            .Should().Throw<ArgumentNullException>();

    }


    [Fact]
    public void ShouldNotAcceptNullUserWhenVerifyingPassword()
    {
        var options = Options.Create(new ImprovedPasswordHasherOptions() { Strength = PasswordHasherStrength.Interactive });
        var passwordHasher = new PasswordHasher<GenericUser>();
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new Scrypt<GenericUser>(passwordHasher, options);

        var hashedPass = scryptHasher.HashPassword(user, password);

        scryptHasher.Invoking(i => i.VerifyHashedPassword(null, hashedPass, password))
            .Should().Throw<ArgumentNullException>();

    }

    [Fact]
    public void ShouldMemLimitSameOfConfiguration()
    {
        var memLimit = _faker.Random.Int(1024, 1073741824);
        var services = new ServiceCollection();
        services.UpgradePasswordSecurity().WithMemLimit(memLimit).UseScrypt<GenericUser>();

        var provider = services.BuildServiceProvider();
        var passwordHasherOptions = (IOptions<ImprovedPasswordHasherOptions>)provider.GetService(typeof(IOptions<ImprovedPasswordHasherOptions>));

        passwordHasherOptions.Value.MemLimit.Should().Be(memLimit);
    }

    [Fact]
    public void ShouldOpsLimitSameOfConfiguration()
    {
        var opsLimit = _faker.Random.Long(3L, 16L);
        var services = new ServiceCollection();
        services.UpgradePasswordSecurity().WithOpsLimit(opsLimit).UseScrypt<GenericUser>();

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
        services.UpgradePasswordSecurity().WithStrengthen(strength).UseScrypt<GenericUser>();

        var provider = services.BuildServiceProvider();
        var passwordHasherOptions = (IOptions<ImprovedPasswordHasherOptions>)provider.GetService(typeof(IOptions<ImprovedPasswordHasherOptions>));

        passwordHasherOptions.Value.Strength.Should().Be(strength);
    }


    [Fact]
    public void ShouldConfigurationUseScrypt()
    {
        var services = new ServiceCollection();
        services.UpgradePasswordSecurity().UseScrypt<GenericUser>();

        var provider = services.BuildServiceProvider();
        var passwordHasher = (IPasswordHasher<GenericUser>)provider.GetService(typeof(IPasswordHasher<GenericUser>));

        passwordHasher.Should().BeOfType<Scrypt<GenericUser>>();
    }

}