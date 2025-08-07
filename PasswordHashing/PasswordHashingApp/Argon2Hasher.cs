using Konscious.Security.Cryptography;
using System.Text;

namespace PasswordHashingApp;

public static class Argon2Hasher
{
  public static string Hash(string password, string storedSalt)
  {
    // Convert password to bytes
    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
    byte[] saltBytes = Convert.FromBase64String(storedSalt);

    // Argon2id configuration
    var argon2 = new Argon2id(passwordBytes)
    {
      Salt = saltBytes,
      DegreeOfParallelism = 8, // Threads
      MemorySize = 65536,      // 64 MB
      Iterations = 4
    };

    // Hashing the password
    byte[] hashBytes = argon2.GetBytes(32); // 256 bits

    // Encoding the hash and salt to Base64 for storage
    string hashBase64 = Convert.ToBase64String(hashBytes);
    return hashBase64;
  }
}
