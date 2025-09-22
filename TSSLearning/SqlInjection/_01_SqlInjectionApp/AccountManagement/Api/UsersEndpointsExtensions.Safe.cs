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
      return Results.Ok(users.Select(u => new UserDto(u.Id, u.Name, u.FirstName, u.Email)));
    });

    app.MapGet("/api/users/{id}", async (IUsersRepository repo, Guid id) =>
    {
      var users = await repo.GetByPatternAsync(id.ToString());
      return Results.Ok(users.Select(u => new UserDto(u.Id, u.Name, u.FirstName, u.Email)));
    });

    app.MapPost("/api/users", async (IUsersRepository repo, UserCreationDto dto) =>
    {
      var user = new User() { Name = dto.Name, FirstName = dto.FirstName, Email = dto.Email };
      await repo.AddAsync(user);
      return Results.Created($"/api/users/{user.Id}", new UserDto(user.Id, user.Name, user.FirstName, user.Email));
    });

  }
}
