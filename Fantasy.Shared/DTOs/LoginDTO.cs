using Fantasy.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace Fantasy.Shared.DTOs;

public class LoginDTO
{
    [Display(Name = "Email", ResourceType = typeof(Resource))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
    [EmailAddress(ErrorMessageResourceName = "ValidEmail", ErrorMessageResourceType = typeof(Resource))]
    public string Email { get; set; } = null!;

    [Display(Name = "Password", ResourceType = typeof(Resource))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
    [MinLength(6, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(Resource))]
    public string Password { get; set; } = null!;
}