using _01_SqlInjectionApp.Users.Application;
using _01_SqlInjectionApp.Users.Domain;
using Microsoft.Data.Sqlite;

namespace _01_SqlInjectionApp.Users.Infrastructure;

public class UsersSqliteSafeRepository : IUsersRepository
{
  private readonly string _connectionString;

  public UsersSqliteSafeRepository(string connectionString)
  {
    if (string.IsNullOrWhiteSpace(connectionString))
      throw new ArgumentNullException(nameof(connectionString));

    _connectionString = connectionString;
  }

  protected List<User> GetByCommand(Action<SqliteCommand> SetCommand)
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

  public Task<List<User>> GetByPatternAsync(string? searchPattern = null)
  {
    return Task.FromResult(GetByCommand(cmd => SetReadUsersCommand(cmd, searchPattern)));
  }

  public Task AddAsync(User user)
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

      // To avoid:
      // x', 'x', (SELECT IFNULL(company, '[no company]') || ' | ' || IFNULL(subject , '[no subject]') || ' | ' || IFNULL(value , '[no value ]') AS name FROM CONTRACTS)); --

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

  public static void SetReadUsersCommand(SqliteCommand readcommand, string? searchPattern = null)
  {
    string sqlBaseQuery = $"SELECT Name, FirstName, Email FROM Users";
    if (string.IsNullOrWhiteSpace(searchPattern))
    {
      readcommand.CommandText = sqlBaseQuery;
      return;
    }

    // To avoid:

    // xxx' UNION SELECT sqlite_version(), 'x', 'x';
    // xxx' UNION SELECT IFNULL(type, '[no type]') || ' | ' || IFNULL(name, '[no name]') || ' | ' || IFNULL(tbl_name, '[no tbl_name]') || ' | ' || IFNULL(sql, '[no sql]') AS name, 'x', 'x' FROM sqlite_master; --
    // xxx' UNION SELECT IFNULL(company, '[no company]') || ' | ' || IFNULL(subject , '[no subject]') || ' | ' || IFNULL(value , '[no value ]') AS name, 'x', 'x' FROM CONTRACTS; --
    // xxx' UNION SELECT IFNULL(key, '[no key]') || ' | ' || IFNULL(hash, '[no hash]') AS name, 'x', 'x' FROM SECRETS; --
    // xxx'; DROP TABLE USERS;  DROP TABLE CONTRACTS; DROP TABLE SECRETS; --


    string where = "WHERE Name LIKE @s OR FirstName LIKE @s OR Email LIKE @s";
    readcommand.CommandText = $"{sqlBaseQuery} {where}";
    readcommand.Parameters.AddWithValue("@s", $"%{searchPattern}%");
  }
}

// TODO :
// 1. ajouter des éléments démontrant que les SqlParameters ne sont pas suffisants, Order By avec chaine directement par exemple
// 2. mettre en place un correctif pour sécuriser ce cas via une validation des entrées.
// 3. Validation des entrées par exclusion et une autre par inclusion (avantages/inconvénients).