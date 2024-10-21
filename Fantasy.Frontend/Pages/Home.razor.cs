using Fantasy.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Fantasy.Frontend.Pages;

public partial class Home
{
    [Inject] private IStringLocalizer<Resource> Localizer { get; set; } = null!;
}