using CurrieTechnologies.Razor.SweetAlert2;
using Fantasy.Frontend.Repositories;
using Fantasy.Shared.DTOs;
using Fantasy.Shared.Entities;
using Fantasy.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;

namespace Fantasy.Frontend.Pages.Teams;

public partial class TeamForm
{
    [Inject] private SweetAlertService Swal { get; set; } = null!;
    [Inject] private IStringLocalizer<Resource> Localizer { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;

    private EditContext editContext = null!;
    private Country selectedCountry = new();
    private List<Country>? countries;
    private string? imageUrl;

    [EditorRequired, Parameter] public TeamDTO TeamDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSumit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    public bool FormPostSuccessfully { get; set; } = false;

    protected override void OnInitialized()
    {
        editContext = new(TeamDTO);
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadCountriesAsync();
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (!string.IsNullOrEmpty(TeamDTO.Image))
        {
            imageUrl = TeamDTO.Image;
            TeamDTO.Image = null;
        }
    }

    private async Task LoadCountriesAsync()
    {
        var responseHttp = await Repository.Get<List<Country>>("/api/countries/combo");
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await Swal.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        countries = responseHttp.Response;
    }

    private void ImageSelected(string imageBase64)
    {
        TeamDTO.Image = imageBase64;
        imageUrl = null;
    }

    private async Task OnBeforeInternalNavigation(LocationChangingContext context)
    {
        var formWasEdited = editContext.IsModified();
        if (!formWasEdited || FormPostSuccessfully)
        {
            return;
        }

        var result = await Swal.FireAsync(new SweetAlertOptions
        {
            Title = Localizer["Confirmation"],
            Text = Localizer["LeaveAndLoseChanges"],
            Icon = SweetAlertIcon.Warning,
            ShowCancelButton = true
        });

        var confirm = !string.IsNullOrEmpty(result.Value);
        if (confirm)
        {
            return;
        }

        context.PreventNavigation();
    }
}