using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Fantasy.Frontend.AuthenticationProviders;

public class AuthenticationProviderTest : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await Task.Delay(4000);
        var anonimous = new ClaimsIdentity();  //un usuario anonimo
        var user = new ClaimsIdentity(authenticationType: "test"); //Usuario autenticado

        //un usuario con Claims
        var admin = new ClaimsIdentity(new List<Claim>
        {
            new Claim("FirstName", "Hebert"),
            new Claim("LastName", "Merchan"),
            new Claim(ClaimTypes.Name, "merchanhebert@gmail.com"),
            new Claim(ClaimTypes.Role, "Admin")
        },
        authenticationType: "test");

        return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user)));
    }
}