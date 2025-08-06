using System.Reflection;

namespace ObfuscationLib.net461
{
  [Obfuscation(Feature = "renaming", ApplyToMembers = true, Exclude = false)]
  public class MySensitiveData
  {
    public const string _mySecretKey = "MySuperSecret";

    public string PrintSecret()
    {
      return $"The secret key is: {_mySecretKey}";
    }
  }
}
