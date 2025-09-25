using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

/// Use Jaeger
var jaeger = builder.AddContainer("jaeger", "jaegertracing/all-in-one")
    .WithEndpoint("http", endpoint => { endpoint.Port = 16686; endpoint.TargetPort = 16686; endpoint.UriScheme = "http"; })     // Interface web Jaeger
    .WithEndpoint("gRPC", endpoint => { endpoint.Port = 4317; endpoint.TargetPort = 4317; endpoint.UriScheme = "http"; })       // OTLP gRPC
    .WithEndpoint("http-oltp", endpoint => { endpoint.Port = 4318; endpoint.TargetPort = 4318; endpoint.UriScheme = "http"; })  // OTLP HTTP
 ;

// Add parameters for username and password
var username = builder.AddParameter("username", "admin");
var password = builder.AddParameter("password", "admin", secret: true);

var keycloak = builder.AddKeycloak("keycloak", 9090, username, password)
  .WithArgs(
  "--tracing-enabled=true", // Enable tracing for monitoring, export by default to http://localhost:4317 in gRPC
  "--tracing-endpoint=http://jaeger:4317", // 👈 Ajout de l'endpoint correct
  "--metrics-enabled=true", // Enable metrics for monitoring
  "--event-metrics-user-enabled=true",
  "--event-metrics-user-events=login,logout",
  "--event-metrics-user-tags=realm,idp,clientId"
  //,"--log-level=INFO,org.keycloak:debug,org.keycloak.events:trace" // Enable detailed logging for debugging
  //,"--log-level=debug" // Enable detailed logging for debugging
  )
  .WithEnvironment("KC_HTTPS_CERTIFICATE_KEY_FILE", "/opt/keycloak/conf/localhost.key.pem")
  .WithEnvironment("KC_HTTPS_CERTIFICATE_FILE", "/opt/keycloak/conf/localhost.crt.pem")
  .WithEnvironment("KC_HTTPS_TRUST_STORE_FILE", "/opt/keycloak/conf/truststore.jks")
  .WithEnvironment("KC_HTTPS_TRUST_STORE_PASSWORD", "password")
  .WithEnvironment("KC_HTTPS_CLIENT_AUTH", "request")
  .WithBindMount("./Certificates/localhost-key.pem", "/opt/keycloak/conf/localhost.key.pem")
  .WithBindMount("./Certificates/localhost-crt.pem", "/opt/keycloak/conf/localhost.crt.pem")
  .WithBindMount("./Certificates/truststore.jks", "/opt/keycloak/conf/truststore.jks")
  .WithRealmImport("./Realms")
  //.WithLifetime(ContainerLifetime.Persistent)
  .WaitFor(jaeger) // Configure Keycloak with a persistent lifetime, useful to avoid long startup times on each run
  ;

builder.AddProject<Projects.AuthenticationCertificateApp>("authenticationcertificateapp")
  .WithReference(keycloak) // Reference Keycloak for authentication
  .WaitFor(keycloak); // Ensure Keycloak is ready before starting

builder.Build().Run();
