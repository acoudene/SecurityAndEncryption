using Microsoft.AspNetCore.Components;
using Microsoft.Data.Sqlite;
using MudBlazor.Services;
using SqlInjectionApp.Client.Proxies;
using SqlInjectionApp.Components;

/// NOTE: don't use Microsoft.EntityFrameworkCore.Sqlite.Core if you want to SQLitePCLRaw core embedded

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<PersonneClient>(sp =>
{
  var nav = sp.GetRequiredService<NavigationManager>();
  return new PersonneClient(new HttpClient { BaseAddress = new Uri(new Uri(nav.BaseUri), "api/Personne/") });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseWebAssemblyDebugging();
}
else
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(SqlInjectionApp.Client._Imports).Assembly);

app.MapControllers();

using (var conn = new SqliteConnection("Data Source=Test.db"))
{
  conn.Open();
  var commande = conn.CreateCommand();
  commande.CommandText = "DROP TABLE IF EXISTS PERSONNES; CREATE TABLE PERSONNES (nom VARCHAR(32), prenom VARCHAR(32), age INT)";
  commande.ExecuteNonQuery();

  commande = conn.CreateCommand();
  commande.CommandText = "DROP TABLE IF EXISTS CONTRATS; CREATE TABLE CONTRATS (entreprise VARCHAR(32), sujet VARCHAR(32), montant INT)";
  commande.ExecuteNonQuery();

  commande = conn.CreateCommand();
  commande.CommandText = "DROP TABLE IF EXISTS USERS; CREATE TABLE USERS (login VARCHAR(32), hash VARCHAR(32))";
  commande.ExecuteNonQuery();

  commande = conn.CreateCommand();
  commande.CommandText = "INSERT INTO PERSONNES (nom, prenom, age) VALUES ('Lagaffe', 'Gaston', 63)";
  commande.ExecuteNonQuery();

  commande = conn.CreateCommand();
  commande.CommandText = "INSERT INTO CONTRATS (entreprise, sujet, montant) VALUES ('Ministère', 'Contrat ultra-secret', 1000000)";
  commande.ExecuteNonQuery();
}

app.Run();
