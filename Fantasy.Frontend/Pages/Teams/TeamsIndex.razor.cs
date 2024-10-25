using CurrieTechnologies.Razor.SweetAlert2;
using Fantasy.Frontend.Repositories;
using Fantasy.Shared.Entities;
using Fantasy.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Fantasy.Frontend.Pages.Teams;

public partial class TeamsIndex
{
    [Inject] private IStringLocalizer<Resource> Localizer { get; set; } = null!;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Inject] private SweetAlertService Swal { get; set; } = null!;

    [Inject] private IRepository Repository { get; set; } = null!;

    private List<Team>? Teams { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        var responseHttp = await Repository.Get<List<Team>>("/api/teams");
        Teams = responseHttp.Response;
    }

    private async Task DeleteAsync(Team Item)
    {
        var result = await Swal.FireAsync(new SweetAlertOptions
        {
            Title = Localizer["Confirmation"],
            Text = string.Format(Localizer["DeleteConfirm"], Localizer["Team"], Item.Name),
            Icon = SweetAlertIcon.Question,
            ShowCancelButton = true,
            CancelButtonText = Localizer["Cancel"]
        });

        var confirm = string.IsNullOrEmpty(result.Value);
        if (confirm)
        {
            return;
        }

        var responseHttp = await Repository.Delete($"/api/teams/{Item.CountryId}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/teams");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                await Swal.FireAsync(Localizer["Error"], Localizer[messageError!], SweetAlertIcon.Error);
            }
            return;
        }

        await LoadAsync();

        var toast = Swal.Mixin(new SweetAlertOptions
        {
            Toast = true,
            Position = SweetAlertPosition.BottomEnd,
            ShowConfirmButton = true,
            Timer = 2000
        });

        await toast.FireAsync(icon: SweetAlertIcon.Success, message: Localizer["RecordDeleteOk"]);
    }
}