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

    // xxx' UNION SELECT sqlite_version(), 'x', 'x';
    // xxx' UNION SELECT IFNULL(type, '[no type]') || ' | ' || IFNULL(name, '[no name]') || ' | ' || IFNULL(tbl_name, '[no tbl_name]') || ' | ' || IFNULL(sql, '[no sql]') AS name, 'x', 'x' FROM sqlite_master; --
    // xxx' UNION SELECT IFNULL(company, '[no company]') || ' | ' || IFNULL(subject , '[no subject]') || ' | ' || IFNULL(value , '[no value ]') AS name, 'x', 'x' FROM CONTRACTS; --
    // xxx' UNION SELECT IFNULL(key, '[no key]') || ' | ' || IFNULL(hash, '[no hash]') AS name, 'x', 'x' FROM SECRETS; --
    // xxx'; DROP TABLE USERS; --


    string where = "WHERE Name LIKE @s OR FirstName LIKE @s OR Email LIKE @s";
    readcommand.CommandText = $"{sqlBaseQuery} {where}";
    readcommand.Parameters.AddWithValue("@s", $"%{searchPattern}%");
  }
}
