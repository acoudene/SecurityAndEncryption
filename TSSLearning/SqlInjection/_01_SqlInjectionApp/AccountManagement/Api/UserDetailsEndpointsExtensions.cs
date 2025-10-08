namespace _01_SqlInjectionApp.AccountManagement.Api;

using _01_SqlInjectionApp.Users.Application;
using System.Net.Mime;
using System.Text;

public static class UserDetailsEndpointsExtensions
{
  public static void MapUserDetailsEndpoints(this IEndpointRouteBuilder app)
  {
    app.MapGet("/api/users/details/{name}", async (IUsersRepository repo, string name) =>
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine("<html>");
      sb.AppendLine("<body>");

      var users = await repo.GetByPatternAsync(name);
      foreach (var user in users)
      {        
        sb
        .Append("<h1>")
        .Append($"{user.FirstName} {user.Name}")
        .AppendLine("</h1>");
        sb
        .AppendLine($"<p>avec l'email {user.Email}</p>");
      }

      sb.AppendLine("</body>");
      sb.AppendLine("</html>");

      return Results.Content(sb.ToString(), MediaTypeNames.Text.Html, Encoding.UTF8);
    });

  }
}
