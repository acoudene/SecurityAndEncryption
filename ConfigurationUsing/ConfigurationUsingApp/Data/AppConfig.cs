using System.ComponentModel.DataAnnotations;

namespace ConfigurationUsingApp.Data;

public record AppConfig
{
  [Required]
  public string Message { get; set; } = string.Empty;

  [Range(1, 10)]
  public int ImportanceLevel { get; set; }
}
