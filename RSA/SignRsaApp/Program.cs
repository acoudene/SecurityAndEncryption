using System.Collections;
using System.Security.Cryptography;
using System.Text;

// RSA data signature example
{
  using RSA rsa = RSA.Create(2048);

  // Message to sign
  string message = "Message to sign";
  byte[] messageBytes = Encoding.UTF8.GetBytes(message);

  // Sign the message with the private key
  byte[] signature = rsa.SignData(messageBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
  Console.WriteLine($"Signature: {Convert.ToBase64String(signature)}");

  // Check the signature with the public key
  bool isValid = rsa.VerifyData(messageBytes, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
  Console.WriteLine($"Is signature valid? {isValid}");
}

// With SHA-256 and RSA hash signature  
{
  using RSA rsa = RSA.Create(2048);

  // Message to sign
  string message = "Message to sign";
  byte[] messageBytes = Encoding.UTF8.GetBytes(message);

  // Step1: Hash the message using SHA-256
  using SHA256 sha256 = SHA256.Create();
  byte[] hash = sha256.ComputeHash(messageBytes);

  // Step 2: Sign the hash with the RSA private key
  byte[] signature = rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
  Console.WriteLine($"Signature: {Convert.ToBase64String(signature)}");

  // Step 3: Verify the signature using the RSA public key
  bool isValid = rsa.VerifyHash(hash, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
  Console.WriteLine($"Is signature valid? {isValid}");  
}
