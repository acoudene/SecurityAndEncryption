namespace SqlInjectionApp.Client.Entities;

/// NOTE: don't use record if this class is used to map to EF (no Clean Architecture here!).
public class Personne
{
  public string? Nom { get; set; }
  public string? Prenom { get; set; }
  public int Age { get; set; }
}

