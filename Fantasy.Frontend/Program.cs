using CurrieTechnologies.Razor.SweetAlert2;
using Fantasy.Frontend;
using Fantasy.Frontend.AuthenticationProviders;
using Fantasy.Frontend.Repositories;
using Fantasy.Frontend.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7227") });

//Se ejecuta los servicios que tenemos Centralizados abajo
ConfigureServices(builder.Services);

await builder.Build().RunAsync();

//La manera organizada es Centralizar la declaracion de Servicios
void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IRepository, Repository>();
    services.AddLocalization();
    services.AddSweetAlert2();
    services.AddMudServices(); //Para Implementar MudBlazor

    //para la autenticacion de usuarios
    services.AddAuthorizationCore();
    //services.AddScoped<AuthenticationStateProvider, AuthenticationProviderTest>();

    //Implementacion del Sistema de Validacion de Usuarios
    services.AddScoped<AuthenticationProviderJWT>();
    services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(
        x => x.GetRequiredService<AuthenticationProviderJWT>());
    services.AddScoped<ILoginService, AuthenticationProviderJWT>(
        x => x.GetRequiredService<AuthenticationProviderJWT>());
}