using Fantasy.Shared.Entities;
using Fantasy.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace Fantasy.Shared.DTOs;

public class TeamDTO
{
    public int TeamId { get; set; }

    [Display(Name = "Team", ResourceType = typeof(Resource))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resource))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
    public string Name { get; set; } = null!;

    [Display(Name = "Country", ResourceType = typeof(Resource))]
    public int CountryId { get; set; }

    [Display(Name = "Image", ResourceType = typeof(Resource))]
    public string? Image { get; set; }
}