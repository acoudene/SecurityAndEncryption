using _01_SqlInjectionApp.AccountManagement.Api;
using _01_SqlInjectionApp.Shared.Api;
using _01_SqlInjectionApp.Shared.Infrastructure;
using _01_SqlInjectionApp.Shared.Ui.Components;
using _01_SqlInjectionApp.Users.Application;
using _01_SqlInjectionApp.Users.Infrastructure;
using Microsoft.AspNetCore.Components;

#region Database prerequisites

string connectionString = $"Data Source=sqlite.db";
var sqliteSeeding = new SqliteSeeding(connectionString);
sqliteSeeding.EnsureDatabases();
sqliteSeeding.Seed();

#endregion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

#region Injections

//builder.Services.AddScoped<IUsersRepository>(sp => new UsersSqliteRepository(connectionString));
builder.Services.AddScoped<IUsersRepository>(sp => new UsersSqliteSafeRepository(connectionString));
builder.Services.AddScoped<UsersProxySafeClient>(serviceProvider =>
{
  var nav = serviceProvider.GetRequiredService<NavigationManager>();
  string? baseUrl = nav?.BaseUri;
  if (string.IsNullOrWhiteSpace(baseUrl))
    throw new InvalidOperationException("Configuration value App:BaseUrl is missing or empty");

  var httpClient = new HttpClient
  {
    BaseAddress = new Uri(new Uri(baseUrl), "/api/users/")
  };
  return new UsersProxySafeClient(httpClient);
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

#region API settings

app.MapUsersEndpoints();
app.MapUserDetailsEndpoints();
app.UseMiddleware<ExceptionHandlingMiddleware>();

#endregion

app.Run();
