using System.ComponentModel.DataAnnotations;
using Fantasy.Shared.Resources;

namespace Fantasy.Shared.Entities;

public class Country
{
    public int CountryId { get; set; }

    [Display(Name = "Country", ResourceType = typeof(Resource))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resource))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
    public string Name { get; set; } = null!;

    //Relaciones
    public ICollection<Team>? Teams { get; set; }

    public ICollection<User>? Users { get; set; }

    public int TeamsCount => Teams == null ? 0 : Teams.Count;
}