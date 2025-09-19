using _01_SqlInjectionApp.Database;
using _01_SqlInjectionApp.Dtos;

namespace _01_SqlInjectionApp.Endpoints;

public static class UsersEndpointsExtensions
{
  public static void MapUsersEndpoints(this IEndpointRouteBuilder app, string connectionString)
  {
    app.MapGet("/api/users", (string? searchPattern)
      => new List<UserDto>().LoadUnsafeReadUsersDb(connectionString, searchPattern));

    app.MapGet("/api/safe/users", (string? searchPattern)
      => new List<UserDto>().LoadSafeReadUsersDb(connectionString, searchPattern));

    app.MapPost("/api/users", (UserCreateDto userCreate) =>
    {
      if (string.IsNullOrWhiteSpace(userCreate.Username) || string.IsNullOrWhiteSpace(userCreate.Email))
        return Results.BadRequest();

      userCreate.StoreUserDb(connectionString);
      return Results.Created("/api/users", userCreate);
    });
  }
}
