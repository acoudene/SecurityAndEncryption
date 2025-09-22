using _01_SqlInjectionApp.IdentityProvider.Api;

namespace _01_SqlInjectionApp.Users.Infrastructure;

public class UsersProxyClient
{
  private readonly HttpClient _httpClient;

  public UsersProxyClient(HttpClient httpClient)
  {
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));    
  }

  public async Task CreateAsync(UserCreationDto userCreation)
  {
    var response = await _httpClient.PostAsJsonAsync<UserCreationDto>(string.Empty, userCreation);
    response.EnsureSuccessStatusCode();
  }

  public async Task<List<UserDto>?> SearchAsync(string? searchPattern = null)
  {
    string requestUri = string.IsNullOrWhiteSpace(searchPattern) ? string.Empty : $"?searchPattern={Uri.EscapeDataString(searchPattern)}";
    var response = await _httpClient.GetFromJsonAsync<List<UserDto>>(requestUri);
    return response;
  }
}
