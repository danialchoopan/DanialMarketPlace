using System.Security.Cryptography;
using System.Text;
using MarketPlaceCore.Core.Interfaces;

namespace MarketPlaceCore.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    public bool Verify(string password, string hash)
    {
        return Hash(password) == hash;
    }
}
