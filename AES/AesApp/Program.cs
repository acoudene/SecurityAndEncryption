using CryptographyProvider;

string mySecret = "This is my secret";
string key = "12345678901234567890123456789012"; // 32 characters key
var provider = new AesCryptographyProvider();
string encryptedKey = provider.Encrypt(mySecret, key);
string decryptedKey = provider.Decrypt(encryptedKey, key);

Console.WriteLine($"Original: {mySecret}");
Console.WriteLine($"Encrypted: {encryptedKey}");
Console.WriteLine($"Decrypted: {decryptedKey}");

