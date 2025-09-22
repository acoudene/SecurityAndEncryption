namespace _01_SqlInjectionApp.Shared.Infrastructure;

using Microsoft.Data.Sqlite;

public class SqliteSeeding
{
  private readonly string _connectionString;

  public SqliteSeeding(string connectionString)
  {
    if (string.IsNullOrWhiteSpace(connectionString))
      throw new ArgumentNullException(nameof(connectionString));

    _connectionString = connectionString;
  }

  public void EnsureDatabases()
  {
    using (var conn = new SqliteConnection(_connectionString))
    {
      conn.Open();
      var cmd = conn.CreateCommand();
      cmd.CommandText = @"DROP TABLE IF EXISTS USERS; 
                          CREATE TABLE USERS (Id TEXT PRIMARY KEY, Name TEXT NOT NULL, Firstname TEXT NOT NULL, Email TEXT NOT NULL);";
      cmd.ExecuteNonQuery();
      cmd.CommandText = @"DROP TABLE IF EXISTS SECRETS; 
                          CREATE TABLE SECRETS (key TEXT PRIMARY KEY, hash TEXT NOT NULL);";      
      cmd.ExecuteNonQuery();
      cmd.CommandText = @"DROP TABLE IF EXISTS CONTRACTS; 
                          CREATE TABLE CONTRACTS (company TEXT PRIMARY KEY, subject TEXT NOT NULL, value LONG NOT NULL);";      
      cmd.ExecuteNonQuery();
    }
  }

  public void Seed()
  {
    using (var conn = new SqliteConnection(_connectionString))
    {
      conn.Open();
      var cmd = conn.CreateCommand();
      cmd.CommandText = $"INSERT INTO USERS (Id, Name, FirstName, Email) VALUES ('{Guid.NewGuid()}', 'Campbell', 'Grace', 'grace.campbell@scotlandyard.uk')";
      cmd.ExecuteNonQuery();
      cmd = conn.CreateCommand();
      cmd.CommandText = $"INSERT INTO USERS (Id, Name, FirstName, Email) VALUES ('{Guid.NewGuid()}', 'Geringën', 'Sarah', 'sarah.geringen@politi.no')";
      cmd.ExecuteNonQuery();
      cmd = conn.CreateCommand();
      cmd.CommandText = $"INSERT INTO SECRETS (key, hash) VALUES ('{Guid.NewGuid()}', 'a97f291da02137d38a5221edf5ad4dba698e5108c545a64c2048b15e21dd0669')";
      cmd.ExecuteNonQuery();
      cmd = conn.CreateCommand();
      cmd.CommandText = "INSERT INTO CONTRACTS (company, subject, value) VALUES ('Ministère', 'Contrat ultra-secret', 1000000)";
      cmd.ExecuteNonQuery();
    }
  }
}
