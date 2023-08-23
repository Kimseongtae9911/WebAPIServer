using System.Security.Cryptography;
using System.Text;

namespace WebAPIServer;

public class Security
{
    private const string SaltCharacter = "abcdefghijklmnopqrstuvwxyz0123456789";
    public static string HashPassword(string saltValue, string password)
    {
        var hash = SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(saltValue + password));
        var stringBuilder = new StringBuilder();

        foreach (var b in hash)
        {
            stringBuilder.AppendFormat("{0:x2}", b);
        }

        return stringBuilder.ToString();
    }

    public static string GetSaltString()
    {
        var bytes = new byte[64];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(bytes);
        }

        return new string(bytes.Select(x => SaltCharacter[x % SaltCharacter.Length]).ToArray());
    }
}
