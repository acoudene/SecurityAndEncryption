var builder = DistributedApplication.CreateBuilder(args);

// Add parameters for username and password
var username = builder.AddParameter("username", "admin");
var password = builder.AddParameter("password", "admin", secret: true);

var keycloak = builder.AddKeycloak("keycloak", 9090, username, password)
  .WithArgs(
  "--tracing-enabled=true", // Enable tracing for monitoring, export by default to http://localhost:4317 in gRPC
//  "--tracing-endpoint=http://jaeger:4317", // 👈 Ajout de l'endpoint correct
  "--metrics-enabled=true", // Enable metrics for monitoring
  "--event-metrics-user-enabled=true",
  "--event-metrics-user-events=login,logout",
  "--event-metrics-user-tags=realm,idp,clientId"
  //,"--log-level=INFO,org.keycloak:debug,org.keycloak.events:trace" // Enable detailed logging for debugging
  //,"--log-level=debug" // Enable detailed logging for debugging
  )
  .WithRealmImport("./Realms")
  //.WithLifetime(ContainerLifetime.Persistent)
  //.WaitFor(jaeger) // Configure Keycloak with a persistent lifetime, useful to avoid long startup times on each run
  ;

builder.AddProject<Projects.AuthenticationCertificateApp>("authenticationcertificateapp")
  .WithReference(keycloak) // Reference Keycloak for authentication
  .WaitFor(keycloak); // Ensure Keycloak is ready before starting

builder.Build().Run();
