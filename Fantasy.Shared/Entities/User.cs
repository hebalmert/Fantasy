using Fantasy.Shared.Enums;
using Fantasy.Shared.Resources;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Fantasy.Shared.Entities;

public class User : IdentityUser
{
    [Display(Name = "FirstName", ResourceType = typeof(Resource))]
    [MaxLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resource))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
    public string FirstName { get; set; } = null!;

    [Display(Name = "LastName", ResourceType = typeof(Resource))]
    [MaxLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resource))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
    public string LastName { get; set; } = null!;

    [Display(Name = "Image", ResourceType = typeof(Resource))]
    public string? Photo { get; set; }

    [Display(Name = "UserType", ResourceType = typeof(Resource))]
    public UserType UserType { get; set; }

    [Display(Name = "Country", ResourceType = typeof(Resource))]
    [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
    public int CountryId { get; set; }

    //Virtuals Connection
    public Country Country { get; set; } = null!;

    [Display(Name = "User", ResourceType = typeof(Resource))]
    public string FullName => $"{FirstName} {LastName}";
}