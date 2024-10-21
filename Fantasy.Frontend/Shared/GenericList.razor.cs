using Fantasy.Frontend.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Fantasy.Frontend.Shared;

public partial class GenericList<Titem>
{
    [Inject] private IStringLocalizer<Resource> Localizer { get; set; } = null!;

    [Parameter] public RenderFragment? Loading { get; set; }

    [Parameter] public RenderFragment? NoRecords { get; set; }

    [Parameter, EditorRequired] public RenderFragment Body { get; set; } = null!;

    [Parameter, EditorRequired] public List<Titem> MyList { get; set; } = null!;
}