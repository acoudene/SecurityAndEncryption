using ConfigurationUsingApp.Data;
using Microsoft.Extensions.Options;

namespace ConfigurationUsingApp.Services;

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
