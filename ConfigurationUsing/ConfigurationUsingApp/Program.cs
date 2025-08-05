// See https://aka.ms/new-console-template for more information

using ConfigurationUsingApp.Data;
using ConfigurationUsingApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration  
  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
  .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
  .AddEnvironmentVariables()
  .AddCommandLine(args);

builder.Services
   .AddOptions<AppConfig>()
   .Bind(builder.Configuration.GetSection("AppConfig"))
   .ValidateDataAnnotations()
   .ValidateOnStart();

builder.Services.AddTransient<AppConfigService>();

var host = builder.Build();

using var cts = new CancellationTokenSource();
var secretUsageService = host.Services.GetRequiredService<AppConfigService>();

// Don't use await to allow the main thread to continue running.  
Task.Run(() =>
{
  Console.WriteLine("Press a key to stop...");
  Console.ReadKey();
  cts.Cancel();
});

var token = cts.Token;
while (!token.IsCancellationRequested)
{
  Console.WriteLine($"Here is my secret: {secretUsageService.GetAppConfig()}");
  await Task.Delay(500);
}

Console.WriteLine("Stop.");
