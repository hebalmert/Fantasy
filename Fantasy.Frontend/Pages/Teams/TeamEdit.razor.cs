using CurrieTechnologies.Razor.SweetAlert2;
using Fantasy.Frontend.Pages.Countries;
using Fantasy.Frontend.Repositories;
using Fantasy.Shared.DTOs;
using Fantasy.Shared.Entities;
using Fantasy.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Fantasy.Frontend.Pages.Teams;

public partial class TeamEdit
{
    private TeamForm? teamForm;

    private TeamDTO? teamDTO;

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private SweetAlertService Swal { get; set; } = null!;
    [Inject] private IStringLocalizer<Resource> Localizer { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.Get<Team>($"/api/teams/{Id}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("countries");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                await Swal.FireAsync(Localizer["Error"], Localizer[messageError!], SweetAlertIcon.Error);
                return;
            }
        }
        else
        {
            var team = responseHttp.Response;
            teamDTO = new()
            {
                TeamId = team!.TeamId,
                Name = team.Name,
                Image = team.Image,
                CountryId = team.CountryId,
            };
        }
    }

    private async Task EditAsync()
    {
        var responseHttp = await Repository.Put("/api/teams/full", teamDTO);
        if (responseHttp.Error)
        {
            var messageError = await responseHttp.GetErrorMessageAsync();
            await Swal.FireAsync(Localizer["Error"], messageError, SweetAlertIcon.Error);
            return;
        }

        Return();

        var toast = Swal.Mixin(new SweetAlertOptions
        {
            Toast = true,
            Position = SweetAlertPosition.BottomEnd,
            ShowConfirmButton = true,
            Timer = 2000
        });

        await toast.FireAsync(icon: SweetAlertIcon.Success, message: Localizer["RecordSavedOk"]);
    }

    private void Return()
    {
        teamForm!.FormPostSuccessfully = true;
        NavigationManager.NavigateTo("/teams");
    }
}