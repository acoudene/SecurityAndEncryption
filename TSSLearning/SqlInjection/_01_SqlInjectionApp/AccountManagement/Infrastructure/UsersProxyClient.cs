using _01_SqlInjectionApp.IdentityProvider.Api;

namespace _01_SqlInjectionApp.Users.Infrastructure;

public class UsersProxyClient
{
  private readonly HttpClient _httpClient;

  public UsersProxyClient(HttpClient httpClient)
  {
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));    
  }

  public async Task<HttpResponseMessage> CreateAsync(UserCreationDto userCreation)
  {
    return await _httpClient.PostAsJsonAsync<UserCreationDto>(string.Empty, userCreation);
  }

  public async Task<List<UserReadDto>?> SearchAsync(string? searchPattern = null)
  {
    string requestUri = string.IsNullOrWhiteSpace(searchPattern) ? string.Empty : $"?searchPattern={searchPattern}";
    return await _httpClient.GetFromJsonAsync<List<UserReadDto>>(requestUri);
  }
}
