namespace ObsucationApp;

public class MySensitiveData
{
  public const string _mySecretKey = "MySuperSecret";

  public string PrintSecret()
  {
    return $"The secret key is: {_mySecretKey}";
  }
}
