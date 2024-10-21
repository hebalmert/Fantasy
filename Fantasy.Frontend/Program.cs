using CurrieTechnologies.Razor.SweetAlert2;
using Fantasy.Frontend;
using Fantasy.Frontend.Repositories;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7227") });

//Se ejecuta los servicios que tenemos Centralizados abajo
ConfigureServices(builder.Services);

await builder.Build().RunAsync();

//La manera organizada es Centralizar la declaracion de Servicios
void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IRepository, Repository>();
    services.AddLocalization();
    services.AddSweetAlert2();
}