// See https://aka.ms/new-console-template for more information

using PasswordHashingApp;

string myPassword = "My Secure Password";

for (int i = 0; i < 10; i++)
{
  // Generate a random salt for each iteration
  string salt = PasswordHelper.GenerateRandomSalt();

  string hash= PasswordHelper.HashPassword(myPassword, salt, Argon2Hasher.Hash);
  Console.WriteLine($"Argon2 - Password: {myPassword}, Hash: {hash}, Salt: {salt}");
  
  bool verified = PasswordHelper.VerifyPassword(myPassword, hash, salt, Argon2Hasher.Hash);
  Console.WriteLine($"Argon2 - Password verified result: {verified}");
}
