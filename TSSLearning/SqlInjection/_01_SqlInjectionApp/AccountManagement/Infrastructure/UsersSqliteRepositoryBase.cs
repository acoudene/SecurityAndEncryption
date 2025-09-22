using _01_SqlInjectionApp.Users.Application;
using _01_SqlInjectionApp.Users.Domain;
using Microsoft.Data.Sqlite;

namespace _01_SqlInjectionApp.Users.Infrastructure;

public abstract class UsersSqliteRepositoryBase : IUsersRepository
{
  private readonly string _connectionString;
  protected string ConnectionString => _connectionString;

  protected UsersSqliteRepositoryBase(string connectionString)
  {
    if (string.IsNullOrWhiteSpace(connectionString))
      throw new ArgumentNullException(nameof(connectionString));

    _connectionString = connectionString;
  }

  protected virtual List<User> GetByCommand(Action<SqliteCommand> SetCommand)
  {
    List<User> readUsers = Enumerable.Empty<User>().ToList();

    using (var conn = new SqliteConnection(_connectionString))
    {
      conn.Open();
      using var cmd = conn.CreateCommand();
      SetCommand(cmd);
      using var reader = cmd.ExecuteReader();
      while (reader.Read())
      {        
        string name = reader.GetString(0);
        string firstName = reader.GetString(1);
        string email = reader.GetString(2);
        readUsers.Add(new User { Name = name, FirstName = firstName, Email = email });
      }
    }

    return readUsers;
  }

  public abstract void SetReadUsersCommand(SqliteCommand readcommand, string? searchPattern = null);
  
  public virtual Task<List<User>> GetByPatternAsync(string? searchPattern = null)
  {
    return Task.FromResult(GetByCommand(cmd => SetReadUsersCommand(cmd, searchPattern)));
  }

  public virtual Task AddAsync(User user)
  {    
    string name = user.Name;
    string firstName = user.FirstName;
    string email = user.Email;
   
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException("Name is required", nameof(user));
    if (string.IsNullOrWhiteSpace(firstName))
      throw new ArgumentException("FirstName is required", nameof(user));
    if (string.IsNullOrWhiteSpace(email))
      throw new ArgumentException("Email is required", nameof(user));

    using (var conn = new SqliteConnection(_connectionString))
    {
      conn.Open();
      using var cmd = conn.CreateCommand();
      cmd.CommandText = "INSERT INTO Users (Name, FirstName, Email) VALUES (@n, @f, @e);";
      cmd.Parameters.AddWithValue("@n", name);
      cmd.Parameters.AddWithValue("@f", firstName);
      cmd.Parameters.AddWithValue("@e", email);
      int rows = cmd.ExecuteNonQuery();
      if (rows != 1)
        throw new InvalidOperationException("Error during user creation");
    }

    return Task.CompletedTask;
  }
}
