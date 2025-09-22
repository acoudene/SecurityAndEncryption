using Microsoft.Data.Sqlite;

namespace _01_SqlInjectionApp.Users.Infrastructure;

public class UsersSqliteSafeRepository : UsersSqliteRepositoryBase
{
  public UsersSqliteSafeRepository(string connectionString)
    : base(connectionString)
  { }

  public override void SetReadUsersCommand(SqliteCommand readcommand, string? searchPattern = null)
  {
    string sqlBaseQuery = $"SELECT Name, FirstName, Email FROM Users";
    if (string.IsNullOrWhiteSpace(searchPattern))
    {
      readcommand.CommandText = sqlBaseQuery;
      return;
    }

    // To avoid:
    // %'; DROP TABLE USERS; --

    string where = "WHERE Name LIKE @s OR FirstName LIKE @s OR Email LIKE @s";
    readcommand.CommandText = $"{sqlBaseQuery} {where}";
    readcommand.Parameters.AddWithValue("@s", $"%{searchPattern}%");
  }
}
