using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SqlInjectionApp.Client.Proxies;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

/// NOTE: Sequence is important, first add instance, then apply httpclient configuration
builder.Services.AddScoped<PersonneClient>();
builder.Services.AddHttpClient<PersonneClient>(httpClient =>
{
  httpClient.BaseAddress = new Uri(new Uri(builder.HostEnvironment.BaseAddress), "/api/Personne/");  
});

await builder.Build().RunAsync();
