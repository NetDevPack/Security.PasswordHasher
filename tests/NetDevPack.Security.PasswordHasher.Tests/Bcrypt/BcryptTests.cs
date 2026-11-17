using Bogus;
using AwesomeAssertions;
using NetDevPack.Security.PasswordHasher.Bcrypt;
using NetDevPack.Security.PasswordHasher.Core;
using NetDevPack.Security.PasswordHasher.Tests.Fakers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Xunit;

namespace NetDevPack.Security.PasswordHasher.Tests.Bcrypt;

public class BcryptTests
{
    private readonly Faker _faker;

    public BcryptTests()
    {
        _faker = new Faker();
    }

    [Fact]
    public void ShouldBeTrueWhenSaltRevision2B()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(new ImprovedPasswordHasherOptions() { SaltRevision = BcryptSaltRevision.Revision2B });

        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new BCrypt<GenericUser>(passwordHasher, options);

        var hashedPass = scryptHasher.HashPassword(user, password);

        scryptHasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
    }

    [Fact]
    public void ShouldBeTrueWhenSaltRevision2()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(new ImprovedPasswordHasherOptions() { SaltRevision = BcryptSaltRevision.Revision2 });

        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new BCrypt<GenericUser>(passwordHasher, options);

        var hashedPass = scryptHasher.HashPassword(user, password);

        scryptHasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
    }


    [Fact]
    public void ShouldBeTrueWhenSaltRevision2A()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(new ImprovedPasswordHasherOptions() { SaltRevision = BcryptSaltRevision.Revision2A });

        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new BCrypt<GenericUser>(passwordHasher, options);

        var hashedPass = scryptHasher.HashPassword(user, password);

        scryptHasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
    }

    [Fact]
    public void ShouldBeTrueWhenSaltRevision2X()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(new ImprovedPasswordHasherOptions() { SaltRevision = BcryptSaltRevision.Revision2X });

        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new BCrypt<GenericUser>(passwordHasher, options);

        var hashedPass = scryptHasher.HashPassword(user, password);

        scryptHasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
    }

    [Fact]
    public void ShouldBeTrueWhenSaltRevision2Y()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(new ImprovedPasswordHasherOptions() { SaltRevision = BcryptSaltRevision.Revision2Y });

        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new BCrypt<GenericUser>(passwordHasher, options);

        var hashedPass = scryptHasher.HashPassword(user, password);

        scryptHasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
    }

    [Fact]
    public void ShouldBeTrueWhenPasswordWithCustomWorkload()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(ImprovedPasswordHasherOptionsFaker.GenerateRandomOptions().Generate());
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate(); ;
        var scryptHasher = new BCrypt<GenericUser>(passwordHasher, options);

        var hashedPass = scryptHasher.HashPassword(user, password);

        scryptHasher.VerifyHashedPassword(user, hashedPass, password).Should().Be(PasswordVerificationResult.Success);
    }

    [Fact]
    public void ShouldNotAcceptNullPasswordWhenHashingPassword()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new BCrypt<GenericUser>(passwordHasher);

        scryptHasher.Invoking(i => i.HashPassword(user, null))
            .Should().Throw<ArgumentNullException>();

    }

    [Fact]
    public void ShouldNotAcceptNullUserWhenHashingPassword()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var password = _faker.Internet.Password();
        var scryptHasher = new BCrypt<GenericUser>(passwordHasher);

        scryptHasher.Invoking(i => i.HashPassword(null, password))
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ShouldNotAcceptNullPasswordWhenVerifyingPassword()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(new ImprovedPasswordHasherOptions() { Strength = PasswordHasherStrength.Interactive });
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new BCrypt<GenericUser>(passwordHasher, options);

        var hashedPass = scryptHasher.HashPassword(user, password);

        scryptHasher.Invoking(i => i.VerifyHashedPassword(user, hashedPass, null))
            .Should().Throw<ArgumentNullException>();

    }


    [Fact]
    public void ShouldNotAcceptNullHashedPasswordWhenVerifyingPassword()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(new ImprovedPasswordHasherOptions() { Strength = PasswordHasherStrength.Interactive });
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new BCrypt<GenericUser>(passwordHasher, options);


        scryptHasher.Invoking(i => i.VerifyHashedPassword(user, null, password))
            .Should().Throw<ArgumentNullException>();

    }


    [Fact]
    public void ShouldNotAcceptNullUserWhenVerifyingPassword()
    {
        var passwordHasher = new PasswordHasher<GenericUser>();
        var options = Options.Create(new ImprovedPasswordHasherOptions() { Strength = PasswordHasherStrength.Interactive });
        var password = _faker.Internet.Password();
        var user = GenericUserFaker.GenerateUser().Generate();
        var scryptHasher = new BCrypt<GenericUser>(passwordHasher, options);

        var hashedPass = scryptHasher.HashPassword(user, password);

        scryptHasher.Invoking(i => i.VerifyHashedPassword(null, hashedPass, password))
            .Should().Throw<ArgumentNullException>();

    }



    [Fact]
    public void ShouldMemLimitSameOfConfiguration()
    {
        var workFactor = _faker.Random.Int(3, 31);
        var services = new ServiceCollection();
        services.UpgradePasswordSecurity().ChangeWorkFactor(workFactor).UseScrypt<GenericUser>();

        var provider = services.BuildServiceProvider();
        var passwordHasherOptions = (IOptions<ImprovedPasswordHasherOptions>)provider.GetService(typeof(IOptions<ImprovedPasswordHasherOptions>));

        passwordHasherOptions.Value.WorkFactor.Should().Be(workFactor);
    }


    [Theory]
    [InlineData(BcryptSaltRevision.Revision2)]
    [InlineData(BcryptSaltRevision.Revision2A)]
    [InlineData(BcryptSaltRevision.Revision2B)]
    [InlineData(BcryptSaltRevision.Revision2X)]
    [InlineData(BcryptSaltRevision.Revision2Y)]
    public void ShouldPasswordStrengthSameOfConfiguration(BcryptSaltRevision saltRevision)
    {
        var services = new ServiceCollection();
        services.UpgradePasswordSecurity().ChangeSaltRevision(saltRevision).UseBcrypt<GenericUser>();

        var provider = services.BuildServiceProvider();
        var passwordHasherOptions = (IOptions<ImprovedPasswordHasherOptions>)provider.GetService(typeof(IOptions<ImprovedPasswordHasherOptions>));

        passwordHasherOptions.Value.SaltRevision.Should().Be(saltRevision);
    }


    [Fact]
    public void ShouldConfigurationUseBcrypt()
    {
        var services = new ServiceCollection();
        services.UpgradePasswordSecurity().UseBcrypt<GenericUser>();

        var provider = services.BuildServiceProvider();
        var passwordHasher = (IPasswordHasher<GenericUser>)provider.GetService(typeof(IPasswordHasher<GenericUser>));

        passwordHasher.Should().BeOfType<BCrypt<GenericUser>>();
    }
}