using CurrieTechnologies.Razor.SweetAlert2;
using Fantasy.Frontend.Repositories;
using Fantasy.Shared.Entities;
using Fantasy.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Fantasy.Frontend.Pages.Countries;

public partial class CountriesEdit
{
    private CountryForm? CountryForm { get; set; }

    private Country? Country;

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private SweetAlertService Swal { get; set; } = null!;
    [Inject] private IStringLocalizer<Resource> Localizer { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.Get<Country>($"/api/countries/{Id}");
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
            Country = responseHttp.Response!;
        }
    }

    private async Task EditAsync()
    {
        var responseHttp = await Repository.Put("/api/countries", Country);
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
        CountryForm!.FormPostSuccessfully = true;
        NavigationManager.NavigateTo("/Countries");
    }
}