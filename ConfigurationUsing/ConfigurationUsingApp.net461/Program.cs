using System;

namespace ConfigurationUsingApp.net461
{
  internal class Program
  {
    static void Main(string[] args)
    {
      string envVar = Environment.GetEnvironmentVariable("AppConfig__Message");

      if (string.IsNullOrEmpty(envVar))
      {
        Console.WriteLine("La variable d'environnement 'AppConfig__Message' n'est pas définie.");
      }
      else
      {
        Console.WriteLine($"Valeur de AppConfig__Message : {envVar}");
      }

    }
  }
}
