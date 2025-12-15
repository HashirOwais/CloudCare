using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace CloudCare.Web.Handlers;


//the handler is used to attach the access token to the outgoing requests to the API
public class CloudCareApiHandler : AuthorizationMessageHandler
{
    public CloudCareApiHandler(IAccessTokenProvider provider, NavigationManager nav, IConfiguration config) 
        : base(provider, nav)
    {
        ConfigureHandler(
            authorizedUrls: new[] { config["api:BaseUrl"] },
            scopes: new[] { "openid", "profile", "email" }
        );
    }
}