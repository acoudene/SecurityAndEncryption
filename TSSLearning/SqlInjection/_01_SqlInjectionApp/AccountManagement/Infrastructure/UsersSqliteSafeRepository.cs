using Microsoft.Data.Sqlite;

namespace _01_SqlInjectionApp.Users.Infrastructure;

public class UsersSqliteSafeRepository : UsersSqliteRepositoryBase
{
  public UsersSqliteSafeRepository(string connectionString)
    : base(connectionString)
  { }

  public override void SetReadUsersCommand(SqliteCommand readcommand, string? searchPattern = null)
  {
    string sqlBaseQuery = $"SELECT Id, Name, FirstName, Email FROM Users";
    if (string.IsNullOrWhiteSpace(searchPattern))
    {
      readcommand.CommandText = sqlBaseQuery;
      return;
    }

    string where = "WHERE Id = @p OR Name LIKE @p OR FirstName LIKE @p OR Email LIKE @p";
    readcommand.CommandText = $"{sqlBaseQuery} {where}";
    readcommand.Parameters.AddWithValue("@p", $"%{searchPattern}%");
  }
}
