using _01_SqlInjectionApp.Users.Domain;

namespace _01_SqlInjectionApp.Users.Application;

public interface IUsersRepository
{
  Task<List<User>> GetByPatternAsync(string? searchPattern = null);
  Task AddAsync(User user);
}
