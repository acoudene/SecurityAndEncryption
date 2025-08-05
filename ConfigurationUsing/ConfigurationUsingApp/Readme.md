# If using Environment Variables as secrets

- Define this variable only for IIS process or application.
- This variable should not be available to system level or other processes.
- Logs should not contain this variable.

# How to set environment variables in Windows

Use the command line or PowerShell to set environment variables:

```powershell
[System.Environment]::SetEnvironmentVariable("AppConfig__Message", "My secret value", "Anthony")
```

But if IIS is used, it is better to set the environment variable in the IIS application pool or web application settings.

Example for IIS application pool with MyPool as pool:

```cmd
%windir%\system32\inetsrv\appcmd set config -section:system.applicationHost/applicationPools /[name='MyPool'].environmentVariables.[name='AppConfig__Message',value='My secret value'] /commit:apphost
```

# How to manage environment variable in C#

## Best approach

Allow access from priorities from less prioritary to higher one:
1. generic json file
2. specific json file for environment
3. environment variables
4. command line arguments

through this line:

```csharp
builder.Configuration  
  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
  .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
  .AddEnvironmentVariables()
  .AddCommandLine(args);
```

Create for example a class `AppConfig` with properties that match the environment variables:

```csharp
public record AppConfig
{
  [Required]
  public string Message { get; set; } = string.Empty;

  [Range(1, 10)]
  public int ImportanceLevel { get; set; }
}
```

For our example, we can use `Message` property as secret.

Here are some examples of how to give this value:
- Using command line arguments: `--AppConfig:Message`
- Using environment variables: `AppConfig__Message`
- Using JSON configuration files: `appsettings.json` or `appsettings.Development.json`

Examples of use:

```json
{
  "profiles": {
    "ConfigurationUsingApp_CLI": {
      "commandName": "Project",
      "commandLineArgs": "--AppConfig:Message=\"Bonjour depuis la ligne de commande !\" --AppConfig:ImportanceLevel=1",
      "environmentVariables": {
        "DOTNET_ENVIRONMENT": "Development",
        "AppConfig__Message": "Bonjour depuis les variables d'environnement !",
        "AppConfig__ImportanceLevel": "2"
      }
    },
    "ConfigurationUsingApp_ENV": {
      "commandName": "Project",
      "environmentVariables": {
        "DOTNET_ENVIRONMENT": "Development",
        "AppConfig__Message": "Bonjour depuis les variables d'environnement !",
        "AppConfig__ImportanceLevel": "2"
      }
    },
    "ConfigurationUsingApp_JSON": {
      "commandName": "Project",
      "environmentVariables": {
        "DOTNET_ENVIRONMENT": "Development"
      }
    }
  }
}
```


Then, a way of using `AppConfig` with message property in a C# service:

```csharp
public class AppConfigService
{
  private readonly IOptionsMonitor<AppConfig> _configMonitor;
  public AppConfigService(IOptionsMonitor<AppConfig> configMonitor)
  {
    _configMonitor = configMonitor ?? throw new ArgumentNullException(nameof(configMonitor));
  }

  public AppConfig GetAppConfig()
  {
    return _configMonitor.CurrentValue;
  }
}
```

By using `IOptionsMonitor<T>`, we can access the current value of the configuration, which will be updated if the underlying configuration changes.
Other kind of options can be used, such as `IOptionsSnapshot<T>` for scoped lifetime or `IOptions<T>` for singleton lifetime.

# How to manage secrets and environment variables in Docker Swarm

## Secret cration

`docker secret create my-secret -`

## Secret declaration in Yaml

```yaml
secrets:
  my-secret:
    external: true
```

## Secret usage in Yaml

```yaml 
services:
  my-service:
    secrets:
      - my-secret
```

## Secret location

`/run/secrets/my-secret`

# How to manage secrest and environment variables in Kubernetes

## Deployment

Get secret as Environment Variable:

```yaml
env:
  - name: AppConfig__Message
    valueFrom:
      secretKeyRef:
        name: my-secret
        key: secret-message
```

## Commands

### Logs

`kubectl logs debian-pod` ou `kubectl logs debian-pod -c nom-du-conteneur`

### Delete Pod

`kubectl delete pod debian-pod`

### Create Pod from Yaml

`kubectl apply -f ./debian-pod.yaml`

### Create Secret

`kubectl create secret generic my-secret --from-literal=secret-message=Message secret`

### Get Secret

`kubectl get secret my-secret -o jsonpath='{.data.secret-message}' | base64 –decode`

### Update Secret

`echo -n Message secret|base64`

### Update Secret with Patch

`kubectl patch secret my-secret -p '{"data": {"secret-message": "YW50aG9ueQ=="}}'`

### Get Secret in YAML Format

`kubectl get secret my-secret -o yaml`