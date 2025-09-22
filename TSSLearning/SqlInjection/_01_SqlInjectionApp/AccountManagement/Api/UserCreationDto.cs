namespace _01_SqlInjectionApp.IdentityProvider.Api;

public record UserCreationDto
{
  public required string Name { get; set; }
  public required string FirstName { get; set; }
  public required string Email { get; set; }
}