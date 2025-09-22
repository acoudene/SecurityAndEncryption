namespace _01_SqlInjectionApp.Users.Domain;

public record User
{  
  public required string Name { get; set; }
  public required string FirstName { get; set; }
  public required string Email { get; set; }
}

