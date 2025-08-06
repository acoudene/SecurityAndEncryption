# Obfuscation

Use this app to test obfuscation.
For the moment, it uses DotFuscator Community and Professional (trial one).

# Objective

Put difficulties to get this secret value:

```csharp
public class MySensitiveData
{
  public const string _mySecretKey = "MySuperSecret";

  public string PrintSecret()
  {
    return $"The secret key is: {_mySecretKey}";
  }
}
```

The program load code, clear assembly, and obfuscate version of it, by renaming and then by encryption.

# Community Edition

Just do renaming of the members and types.

# Professional Edition

Do encryption of the members and types.