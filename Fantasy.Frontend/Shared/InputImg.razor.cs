using Fantasy.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;

namespace Fantasy.Frontend.Shared;

public partial class InputImg
{
    private string? imageBase64;
    private string? fileName;

    [Inject] private IStringLocalizer<Resource> Localizer { get; set; } = null!;

    [Parameter] public string? Label { get; set; }
    [Parameter] public string? ImageUrl { get; set; }
    [Parameter] public EventCallback<string> ImageSelect { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (string.IsNullOrWhiteSpace(Label))
        {
            Label = Localizer["Image"];
        }
    }

    private async Task OnChange(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null)
        {
            fileName = file.Name;

            var arrBytes = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(arrBytes);
            imageBase64 = Convert.ToBase64String(arrBytes);
            ImageUrl = null;
            await ImageSelect.InvokeAsync(imageBase64);
            StateHasChanged();
        }
    }
}