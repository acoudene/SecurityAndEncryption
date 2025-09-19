using _01_SqlInjectionApp.Components.Pages;
using _01_SqlInjectionApp.Dtos;
using Microsoft.Data.Sqlite;

namespace _01_SqlInjectionApp.Database;

public static class UsersDbExtensions
{
  public static void EnsureDatabase(this WebApplication app, string connectionString)
  {
    // Initialisation DB
    using (var conn = new SqliteConnection(connectionString))
    {
      conn.Open();
      var cmd = conn.CreateCommand();
      cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Users 
                          ( Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Username TEXT NOT NULL,
                            Email TEXT NOT NULL );";
      cmd.ExecuteNonQuery();
    }
  }

  public static void StoreUserDb(this UserCreateDto userCreate, string connectionString)
  {
    using (var conn = new SqliteConnection(connectionString))
    {
      conn.Open();
      using var cmd = conn.CreateCommand();
      cmd.CommandText = "INSERT INTO Users (Username, Email) VALUES (@u, @e);";
      cmd.Parameters.AddWithValue("@u", userCreate.Username);
      cmd.Parameters.AddWithValue("@e", userCreate.Email);
      cmd.ExecuteNonQuery();
    }
  }

  private static void LoadReadUsersDb(
    this List<UserDto> readUsers,
    string connectionString,
    string? searchPattern,
    Action<SqliteCommand, string?> SetCommand)
  {
    readUsers.Clear();
    using (var conn = new SqliteConnection(connectionString))
    {
      conn.Open();
      using var cmd = conn.CreateCommand();
      SetCommand(cmd, searchPattern);
      using var reader = cmd.ExecuteReader();
      while (reader.Read())
      {
        int id = reader.GetInt32(0);
        string username = reader.GetString(1);
        string email = reader.GetString(2);
        readUsers.Add(new UserDto(Id: id, Username: username, Email: email));
      }
    }
  }

  public static void LoadUnsafeReadUsersDb(
    this List<UserDto> readUsers,
    string connectionString,
    string? searchPattern)
  => readUsers.LoadReadUsersDb(connectionString, searchPattern, SetUnsafeReadUserCommand);

  public static void LoadSafeReadUsersDb(
    this List<UserDto> readUsers,
    string connectionString,
    string? searchPattern)
  => readUsers.LoadReadUsersDb(connectionString, searchPattern, SetSafeReadUserCommand);

  public static void SetUnsafeReadUserCommand(this SqliteCommand readcommand, string? searchPattern)
  {
    string sqlQueryBase = $"SELECT Id, Username, Email FROM Users";
    if (string.IsNullOrWhiteSpace(searchPattern))
    {
      readcommand.CommandText = sqlQueryBase;
      return;
    }
    string where = $"WHERE Username LIKE '%{searchPattern}%' OR Email LIKE '%{searchPattern}%'";
    readcommand.CommandText = $"{sqlQueryBase} {where}";
  }

  public static void SetSafeReadUserCommand(this SqliteCommand readcommand, string? searchPattern)
  {
    string sqlQueryBase = $"SELECT Id, Username, Email FROM Users";
    if (string.IsNullOrWhiteSpace(searchPattern))
    {
      readcommand.CommandText = sqlQueryBase;
      return;
    }

    string where = "WHERE Username LIKE @p OR Email LIKE @p";
    readcommand.CommandText = $"{sqlQueryBase} {where}";
    readcommand.Parameters.AddWithValue("@p", $"%{searchPattern}%");
  }
}
