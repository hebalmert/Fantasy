using System.ComponentModel.DataAnnotations;

namespace Fantasy.Shared.Entities;

public class Country
{
    public int CountryId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
}