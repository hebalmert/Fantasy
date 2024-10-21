using Fantasy.Frontend.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Fantasy.Frontend.Layout;

public partial class MainLayout
{
    [Inject] private IStringLocalizer<Resource> Localizer { get; set; } = null!;
}