namespace PasswordHashingApp;

using Konscious.Security.Cryptography;
using System;
using System.Security.Cryptography;
using System.Text;

public static class PasswordHelper
{
  public static (string Hash, string Salt) HashPassword(string password, Func<string, string, string> hash)
  {
    // Random salt generation
    byte[] saltBytes = new byte[16];
    using (var rng = RandomNumberGenerator.Create())
    {
      rng.GetBytes(saltBytes);
    }

    string saltBase64 = Convert.ToBase64String(saltBytes);
    string hashBase64 = hash(password, saltBase64);

    return (hashBase64, saltBase64);
  } 

  public static bool VerifyPassword(string password, string storedHash, string storedSalt, Func<string, string, string> hash)
  {
    string hashBase64 = hash(password, storedSalt);
    return hashBase64 == storedHash;
  }
}

