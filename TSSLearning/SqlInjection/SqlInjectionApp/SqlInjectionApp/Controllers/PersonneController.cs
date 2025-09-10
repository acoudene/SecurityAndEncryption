using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SqlInjectionApp.Client.Entities;

namespace SqlInjectionApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonneController : ControllerBase
{
  [HttpPost]
  public IActionResult CreationPersonne([FromBody] Personne personne)
  {
    try
    {
      using (var conn = new SqliteConnection("Data Source=test.db"))
      {
        conn.Open();
        var commande = conn.CreateCommand();
        commande.CommandText = "INSERT INTO PERSONNES (nom, prenom, age) VALUES ('" + personne.Nom + "', '" + personne.Prenom + "', " + personne.Age.ToString() + ")";
        commande.ExecuteNonQuery();
      }
      return new CreatedResult("#", personne);
    }
    catch (Exception ex)
    {
      return new UnprocessableEntityObjectResult(ex.ToString());
    }
  }

  [HttpGet]
  public Tuple<List<Personne>, string> GetAll([FromQuery] string? IndicationNom)
  {
    var donnees = new List<Personne>();
    string erreur = string.Empty;

    try
    {
      using (var conn = new SqliteConnection("Data Source=test.db"))
      {
        conn.Open();
        var commande = conn.CreateCommand();
        commande.CommandText = "SELECT nom, prenom, age FROM PERSONNES WHERE nom LIKE '%" + IndicationNom + "%'";

        using (var reader = commande.ExecuteReader())
        {
          while (reader.Read())
          {
            donnees.Add(
                new Personne()
                {
                  Nom = reader.GetString(0),
                  Prenom = reader.GetString(1),
                  Age = reader.GetInt32(2)
                });
          }
        }
      }
    }
    catch (Exception ex)
    {
      erreur = ex.ToString();
    }
    return new Tuple<List<Personne>, string>(donnees, erreur);
  }
}
