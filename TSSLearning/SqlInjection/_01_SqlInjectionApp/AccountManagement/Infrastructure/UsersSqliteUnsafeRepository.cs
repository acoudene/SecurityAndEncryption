using Microsoft.Data.Sqlite;

namespace _01_SqlInjectionApp.Users.Infrastructure;

public class UsersSqliteUnsafeRepository : UsersSqliteRepositoryBase
{
  public UsersSqliteUnsafeRepository(string connectionString)
    : base(connectionString)
  { }

  public override void SetReadUsersCommand(SqliteCommand readcommand, string? searchPattern = null)
  {
    string sqlBaseQuery = $"SELECT Id, Name, FirstName, Email FROM Users";
    if (string.IsNullOrWhiteSpace(searchPattern))
    {
      readcommand.CommandText = $"{sqlBaseQuery};";
      return;
    }
    string where = $"WHERE id LIKE '%{searchPattern}%' OR Name LIKE '%{searchPattern}%' OR FirstName LIKE '%{searchPattern}%' OR Email LIKE '%{searchPattern}%';";
    readcommand.CommandText = $"{sqlBaseQuery} {where}";
  }
}
