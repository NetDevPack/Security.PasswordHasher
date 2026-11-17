using AwesomeAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NetDevPack.Security.PasswordHasher.Argon2;
using NetDevPack.Security.PasswordHasher.Bcrypt;
using NetDevPack.Security.PasswordHasher.Core;
using NetDevPack.Security.PasswordHasher.Scrypt;
using System;
using Xunit;

namespace NetDevPack.Security.PasswordHasher.Tests.Compatibility;

/// <summary>
/// Hashes produced by released versions (7.1.2, net8/net9/net10, Sodium.Core 1.4.0).
/// They are persisted in users' databases, so every future version must keep verifying them.
/// Never regenerate these values: a failure here means a breaking change for existing users.
/// </summary>
public class StoredHashCompatibilityTests
{
    private const string Password = "NetDevPack@2026!";

    private const string Argon2IdHash = "$argon2id$v=19$m=32768,t=4,p=1$pd5q71+pxdBYWOWr0WHURg$w7UAva7pdfi6K0OhRzqjjcG9EgoJq9eBqVoy45qI3k4";
    private const string ScryptHash = "$7$C6..../....Sw9xiX8D8fUQICwIxbJ3yURuwUcydVkCdLbInYMKL3A$9B5V6ea2FfSmJYHS9zMorQ1EU0h2IhdlcWjl6tXaXvA";
    private const string AspNetIdentityV3Hash = "AQAAAAIAAYagAAAAEBEIj9XNFNCMoIgudDswzLWiJMbL4a9My9d16z4d5XQaZU8lyGc06t5hW0fzJLyM0A==";
    private const string AspNetIdentityV2Hash = "AHtzSDgvdpCQ+2cOnYy4c40mVHtcwX9FubeRRCCfzZy59sxQMCKiRKuk3wuuR7n00g==";

    private static readonly GenericUser User = new();

    private static IPasswordHasher<GenericUser> Argon2() => new Argon2Id<GenericUser>(new PasswordHasher<GenericUser>());
    private static IPasswordHasher<GenericUser> Scrypt() => new Scrypt<GenericUser>(new PasswordHasher<GenericUser>());
    private static IPasswordHasher<GenericUser> BCrypt() => new BCrypt<GenericUser>(new PasswordHasher<GenericUser>());

    public static TheoryData<string> Hashers => new() { "argon2", "scrypt", "bcrypt" };

    private static IPasswordHasher<GenericUser> Create(string name) => name switch
    {
        "argon2" => Argon2(),
        "scrypt" => Scrypt(),
        "bcrypt" => BCrypt(),
        _ => throw new ArgumentOutOfRangeException(nameof(name))
    };

    [Fact]
    public void Argon2_ShouldVerifyStoredHash()
    {
        Argon2().VerifyHashedPassword(User, Argon2IdHash, Password).Should().Be(PasswordVerificationResult.Success);
        Argon2().VerifyHashedPassword(User, Argon2IdHash, "wrong-password").Should().Be(PasswordVerificationResult.Failed);
    }

    [Fact]
    public void Scrypt_ShouldVerifyStoredHash()
    {
        Scrypt().VerifyHashedPassword(User, ScryptHash, Password).Should().Be(PasswordVerificationResult.Success);
        Scrypt().VerifyHashedPassword(User, ScryptHash, "wrong-password").Should().Be(PasswordVerificationResult.Failed);
    }

    [Theory]
    [InlineData("$2a$04$BYD7oqub9f89KabtZo2YlO.VMdveJEfDWbldNmtNkbwI97VjH8p.u")]
    [InlineData("$2b$04$I.40oldQQT58YawO7WmQVuxLosrUtNhNhqhJU6qRbghpe5fxNdr7a")]
    [InlineData("$2y$04$.5ZIsWyHemKEx0f.N5GzEO1hKv2TmQa3xlPPoHw6SFiKjRA7ggWOW")]
    public void BCrypt_ShouldVerifyStoredHash(string hash)
    {
        BCrypt().VerifyHashedPassword(User, hash, Password).Should().Be(PasswordVerificationResult.Success);
        BCrypt().VerifyHashedPassword(User, hash, "wrong-password").Should().Be(PasswordVerificationResult.Failed);
    }

    [Theory]
    [MemberData(nameof(Hashers))]
    public void ShouldAskForRehashWhenStoredHashIsAspNetIdentityV3(string hasher)
    {
        Create(hasher).VerifyHashedPassword(User, AspNetIdentityV3Hash, Password).Should().Be(PasswordVerificationResult.SuccessRehashNeeded);
        Create(hasher).VerifyHashedPassword(User, AspNetIdentityV3Hash, "wrong-password").Should().Be(PasswordVerificationResult.Failed);
    }

    [Theory]
    [MemberData(nameof(Hashers))]
    public void ShouldAskForRehashWhenStoredHashIsAspNetIdentityV2(string hasher)
    {
        Create(hasher).VerifyHashedPassword(User, AspNetIdentityV2Hash, Password).Should().Be(PasswordVerificationResult.SuccessRehashNeeded);
        Create(hasher).VerifyHashedPassword(User, AspNetIdentityV2Hash, "wrong-password").Should().Be(PasswordVerificationResult.Failed);
    }
}
