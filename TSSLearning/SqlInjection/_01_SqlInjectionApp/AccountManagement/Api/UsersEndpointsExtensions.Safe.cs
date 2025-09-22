namespace _01_SqlInjectionApp.AccountManagement.Api;

using _01_SqlInjectionApp.IdentityProvider.Api;
using _01_SqlInjectionApp.Users.Application;
using _01_SqlInjectionApp.Users.Domain;

public static class UsersEndpointsSafeExtensions
{
  public static void MapUsersSafeEndpoints(this IEndpointRouteBuilder app)
  {
    app.MapGet("/api/users", async (IUsersRepository repo, string? searchPattern) =>
    {
      var users = await repo.GetByPatternAsync(searchPattern);
      return Results.Ok(users.Select(u => new UserReadDto(u.Name, u.FirstName, u.Email)));
    });

    app.MapGet("/api/users/{name}", async (IUsersRepository repo, string name) =>
    {
      var users = await repo.GetByPatternAsync(name);
      return Results.Ok(users.Select(u => new UserReadDto(u.Name, u.FirstName, u.Email)));
    });

    app.MapPost("/api/users", async (IUsersRepository repo, UserCreationDto dto) =>
    {
      var user = new User() { Name = dto.Name, FirstName = dto.FirstName, Email = dto.Email };
      await repo.AddAsync(user);
      return Results.Created($"/api/users/{user.Name}", new UserReadDto(user.Name, user.FirstName, user.Email));
    });

  }
}
