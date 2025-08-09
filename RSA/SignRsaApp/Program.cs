using System.Security.Cryptography;
using System.Text;

using RSA rsa = RSA.Create(2048);

// Message to sign
string message = "Message to sign";
byte[] messageBytes = Encoding.UTF8.GetBytes(message);

// Sign the message with the private key
byte[] signature = rsa.SignData(messageBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
Console.WriteLine("Signature: " + Convert.ToBase64String(signature));

// Check the signature with the public key
bool isValid = rsa.VerifyData(messageBytes, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
Console.WriteLine("Is signature valid? " + isValid);
