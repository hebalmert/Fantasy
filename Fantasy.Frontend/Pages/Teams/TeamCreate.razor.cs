using CurrieTechnologies.Razor.SweetAlert2;
using Fantasy.Frontend.Repositories;
using Fantasy.Shared.DTOs;
using Fantasy.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Fantasy.Frontend.Pages.Teams;

public partial class TeamCreate
{
    private TeamForm? teamForm;

    private TeamDTO teamDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private SweetAlertService Swal { get; set; } = null!;
    [Inject] private IStringLocalizer<Resource> Localizer { get; set; } = null!;

    private async Task CreateAsync()
    {
        var responseHttp = await Repository.Post("/api/teams/full", teamDTO);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await Swal.FireAsync(Localizer["Error"], Localizer[message!], SweetAlertIcon.Error);
            return;
        }

        Return();

        var toast = Swal.Mixin(new SweetAlertOptions
        {
            Toast = true,
            Position = SweetAlertPosition.BottomEnd,
            ShowConfirmButton = true,
            Timer = 3000
        });
        await toast.FireAsync(icon: SweetAlertIcon.Success, message: Localizer["RecordCreatedOk"]);
    }

    private void Return()
    {
        teamForm!.FormPostSuccessfully = true;
        NavigationManager.NavigateTo("/teams");
    }
}