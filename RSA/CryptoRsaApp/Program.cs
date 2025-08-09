
// Génération d'une paire de clés RSA
using System.Security.Cryptography;
using System.Text;

using RSA rsa = RSA.Create(2048);

// Key exportation
string publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
string privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

Console.WriteLine("Public key: " + publicKey);
Console.WriteLine("Private key: " + privateKey);

// Message to encrypt
string message = "Hi Anthony!";
byte[] messageBytes = Encoding.UTF8.GetBytes(message);

for (int i = 0; i < 3; i++)
{
  // Cryptography with the public key
  byte[] encryptedBytes = rsa.Encrypt(messageBytes, RSAEncryptionPadding.OaepSHA256);
  Console.WriteLine("Crypted message: " + Convert.ToBase64String(encryptedBytes));

  // With the private key, decrypting the message
  byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);
  string decryptedMessage = Encoding.UTF8.GetString(decryptedBytes);
  Console.WriteLine("Decrypted message: " + decryptedMessage);
}
//Fonctionnement simplifié de OAEP :
//- Message original(M) est préparé.
//- Un masque est généré à partir d'une source aléatoire (appelée seed).
//- Ce masque est appliqué au message via une fonction de hachage (souvent SHA-1 ou SHA-256).
//- Le message masqué est ensuite combiné avec le seed masqué.
//- Le tout est chiffré avec RSA.
//Ce processus garantit que :
//- Le même message chiffré deux fois donnera des résultats différents (grâce au seed aléatoire).
//- Le message est protégé contre les attaques structurelles.
//✅ Avantages de OAEP :
//- Sécurité renforcée contre les attaques adaptatives.
//- Compatibilité avec les standards modernes (PKCS#1).
//- Utilisation courante dans les bibliothèques comme .NET, Java, OpenSSL.