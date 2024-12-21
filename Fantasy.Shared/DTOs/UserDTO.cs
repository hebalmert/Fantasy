using Fantasy.Shared.Entities;
using Fantasy.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace Fantasy.Shared.DTOs;

public class UserDTO : User
{
    [DataType(DataType.Password)]
    [Display(Name = "Password", ResourceType = typeof(Resource))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
    [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = "LengthField", ErrorMessageResourceType = typeof(Resource))]
    public string Password { get; set; } = null!;

    [Compare("Password", ErrorMessageResourceName = "PasswordAndConfirmationDifferent", ErrorMessageResourceType = typeof(Resource))]
    [Display(Name = "PasswordConfirm", ResourceType = typeof(Resource))]
    [DataType(DataType.Password)]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Resource))]
    [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = "LengthField", ErrorMessageResourceType = typeof(Resource))]
    public string PasswordConfirm { get; set; } = null!;

    public string Language { get; set; } = null!;
}