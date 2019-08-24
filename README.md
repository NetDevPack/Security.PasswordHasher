# Improved PasswordHasher
-------------------------

Custom PasswordHasher for ASP.NET Core Identity. There are 3 options: Argon2id, Scrypt and Bcrypt.

A strong password storage strategy is critical to mitigating data breaches that put the reputation of any organization in danger. Hashing is the foundation of secure password storage.

## Table of Contents ##

- [Why](#why)
- [Download](#download)
- [Configure](#Configure)
    - [Argon2](#argon2)
    - [bcrypt](#bcrypt)
    - [scrypt](#scrypt)
- [Playground](#playground)
- [License](#license)

------------------

# Why? #

ASP.NET Core Identity uses PBKDF2. With HMAC-SHA256. A 128-bit salt. 256-bit subkey and 10.000 iterations. It's FIPS compliant and recommended by NIST. Whilst it's considered good enough, isn't the best choice against newer atack. Such as GPU based.

Wanna know more why Hash password? [Read here](https://crackstation.net/hashing-security.htm) or [here](https://auth0.com/blog/hashing-passwords-one-way-road-to-security/).

----------------

# Download #

The latest stable release of the JPProject PasswordHasher is available on NuGet or can be downloaded from GitHub.

Versions:
* [Argon2](https://www.nuget.org/packages/JpProject.AspNetCore.PasswordHasher.Argon2/) (Most recommended by OWASP)
* [BCrypt](https://www.nuget.org/packages/JpProject.AspNetCore.PasswordHasher.Bcrypt/)
* [Scrypt](https://www.nuget.org/packages/JpProject.AspNetCore.PasswordHasher.Scrypt/)

----------------

# Configure #

There are specific configuration for each one of algorithms.

## Argon2 ##

Argon2 is the winner of the [password hashing competition](https://password-hashing.net/) and should be considered as your first choice for new applications.

Argon2 is cryptographic hashing algorithm, most recommended for password hashing. It is designed by Alex Biryukov, Daniel Dinu, and Dmitry Khovratovich from University of Luxembourg. 

This implementation uses libsodium library and it's implementation of Argon2id. Which is considere best option for Password hashing. 

```
    services.AddDefaultIdentity<IdentityUser>();
    services.UpgradePasswordSecurity().UseArgon2<IdentityUser>();
```

For options (Default is Sensitive, the stronger)

```
    services.UpgradePasswordSecurity()
                    .WithStrenghten(PasswordHasherStrenght.Interactive)
                    .UseArgon2<IdentityUser>();
```

Or more advanced options:

```
    services.UpgradePasswordSecurity()
                    .WithMemLimit(33554432)
                    .WithOpsLimit(4L)
                    .UseArgon2<IdentityUser>();
```
* [Docs](https://password-hashing.net/argon2-specs.pdf)
* [OWASP first recommendation](https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html)
* [Argon2](https://tools.ietf.org/html/draft-irtf-cfrg-argon2-06)

## BCrypt ##

[bcryps](https://en.wikipedia.org/wiki/Bcrypt) was designed by reusing and expanding elements of a block cipher called Blowfish. The iteration count is a power of two, which is a tad less configurable than PBKDF2, but sufficiently so nevertheless. This is the core password hashing mechanism in the OpenBSD operating system.

This implementation uses libsodium library and it's implementation of Argon2id. Which is considere best option for Password hashing. 

```
    services.AddDefaultIdentity<IdentityUser>();
    services.UpgradePasswordSecurity().UseBcrypt<IdentityUser>();
```

For options

```
    services.UpgradePasswordSecurity()
                    .ChangeSaltRevision(BcryptSaltRevision.Revision2B) // default: BcryptSaltRevision.Revision2B
                    .ChangeWorkFactor(15) // default: 10
                    .UseBcrypt<IdentityUser>();
```


## Scrypt ##

scrypt is a much newer construction (designed in 2009) which builds over PBKDF2 and a stream cipher called Salsa20/8, but these are just tools around the core strength of scrypt, which is RAM. scrypt has been designed to inherently use a lot of RAM (it generates some pseudo-random bytes, then repeatedly read them in a pseudo-random sequence). "Lots of RAM" is something which is hard to make parallel. A basic PC is good at RAM access, and will not try to read dozens of unrelated RAM bytes simultaneously. An attacker with a GPU or a FPGA will want to do that, and will find it difficult.


```
    services.AddDefaultIdentity<IdentityUser>();
    services.UpgradePasswordSecurity().UseScrypt<IdentityUser>();
```

For options (Default is Sensitive, the stronger)

```
    services.UpgradePasswordSecurity()
                    .WithStrenghten(PasswordHasherStrenght.Interactive)
                    .UseScrypt<IdentityUser>();
```

Or more advanced options:

```
    services.UpgradePasswordSecurity()
                    .WithMemLimit(33554432)
                    .WithOpsLimit(4L)
                    .UseScrypt<IdentityUser>();
```
------------

# Playground

Wanna see Argon2, Scrypt or BCrypt in action?

* [Argon2 Online](https://argon2.online/)
* [Bcrypt Generator](https://bcrypt-generator.com/)
* [Scrypt Generator](https://scrypt-generator.com/)

---------------

# License
---------
JPProject.PasswordHasher is Open Source software and is released under the MIT license. This license allow the use of JPProject.PasswordHasher in free and commercial applications and libraries without restrictions.

