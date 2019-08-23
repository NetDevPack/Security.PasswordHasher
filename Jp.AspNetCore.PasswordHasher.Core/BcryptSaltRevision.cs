namespace Jp.AspNetCore.PasswordHasher.Core
{
    /// <summary>
    /// The revision to return in the salt portion, defaults to 2b
    /// </summary>
    public enum BcryptSaltRevision
    {
        Revision2,
        Revision2A,
        Revision2B,
        Revision2X,
        Revision2Y
    }
}
