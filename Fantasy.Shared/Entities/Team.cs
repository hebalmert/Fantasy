using Fantasy.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace Fantasy.Shared.Entities;

public class Team
{
    public int TeamId { get; set; }

    [Display(Name = "Team", ResourceType = typeof(Resource))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resource))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
    public string Name { get; set; } = null!;

    public int CountryId { get; set; }

    public string? Image { get; set; }

    public string ImageFull => string.IsNullOrEmpty(Image) ? "/images/NoImage.png" : Image;

    //Relaciones
    public Country? Country { get; set; }
}