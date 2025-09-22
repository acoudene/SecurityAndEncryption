namespace _01_SqlInjectionApp.Shared.Api;

using System.Net.Mime;

public class ExceptionHandlingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ExceptionHandlingMiddleware> _logger;

  public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (ArgumentException ex)
    {
      _logger.LogWarning(ex, "Bad request");
      await WriteResultAsync(context,
          Results.Problem(
              detail: ex.Message,
              statusCode: StatusCodes.Status400BadRequest,
              title: "Bad Request"
          ));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Internal Server Error");
      await WriteResultAsync(context,
          Results.Problem(
              detail: "An unexpected error occurred.",
              statusCode: StatusCodes.Status500InternalServerError,
              title: "Internal Server Error"
          ));
    }
  }

  private static async Task WriteResultAsync(HttpContext context, IResult result)
  {
    context.Response.ContentType = MediaTypeNames.Application.Json;
    await result.ExecuteAsync(context);
  }
}
