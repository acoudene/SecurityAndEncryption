namespace _01_SqlInjectionApp.AccountManagement.Api;

using _01_SqlInjectionApp.Users.Application;
using System.Net.Mime;
using System.Text;

public static class UserDetailsEndpointsSafeExtensions
{
  public static void MapUserDetailsSafeEndpoints(this IEndpointRouteBuilder app)
  {
    // TODO
    // Field with: 
    // <td>Ah ah ah <img src="https://www.powertrafic.fr/wp-content/uploads/2023/04/image-ia-exemple.png" onload="alert('virus !')"/></td>
    
    app.MapGet("/api/users/details/{name}", async (IUsersRepository repo, string name) =>
    {
      StringBuilder sb = new StringBuilder();
      sb
      .AppendLine("<html>")
      .AppendLine("<body>");

      var users = await repo.GetByPatternAsync(name);
      foreach (var user in users)
      {        
        sb
        .Append("<h1>")
        .Append($"{user.FirstName} {user.Name}")
        .AppendLine("</h1>")
        .AppendLine($"<p>Email: {user.Email}</p>");
      }

      sb
      .AppendLine("</body>")
      .AppendLine("</html>");

      return Results.Content(sb.ToString(), MediaTypeNames.Text.Html, Encoding.UTF8);
    });

  }
}
