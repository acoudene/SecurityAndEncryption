using SqlInjectionApp.Client.Entities;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace SqlInjectionApp.Client.Proxies;

public class PersonneClient
{
  private readonly HttpClient _httpClient;

  public PersonneClient(HttpClient httpClient)
  {
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    Console.WriteLine($"BaseAddress: {httpClient.BaseAddress}");
  }
  public async Task CreateClientAsync(Personne personneEnCreation)
  {
    var response = await _httpClient.PostAsJsonAsync<Personne>(string.Empty, personneEnCreation);
    response.EnsureSuccessStatusCode();
  }

  public async Task<Personne[]?> GetPersonnesAsync(string filtre)
  {
    var result = await _httpClient.GetFromJsonAsync<Tuple<Personne[], string>>(filtre);
    return result?.Item1.ToArray();
  }
}
