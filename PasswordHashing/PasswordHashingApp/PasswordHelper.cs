namespace PasswordHashingApp;

using System;
using System.Security.Cryptography;

public static class PasswordHelper
{
  public static string GenerateRandomSalt(int length = 16)
  {
    if (length <= 0)
      throw new ArgumentException("Salt length must be greater than zero.", nameof(length));
    byte[] saltBytes = new byte[length];
    using (var rng = RandomNumberGenerator.Create())
    {
      rng.GetBytes(saltBytes);
    }
    return Convert.ToBase64String(saltBytes);
  }

  public static string HashPassword(string password, string salt, Func<string, string, string> hash)
  {
    return hash(password, salt);
  }

  public static bool VerifyPassword(string password, string storedHash, string storedSalt, Func<string, string, string> hash)
  {
    return hash(password, storedSalt) == storedHash;
  }
}

