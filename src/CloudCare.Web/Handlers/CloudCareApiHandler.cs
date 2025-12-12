using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace CloudCare.Web.Handlers;


public class CloudCareApiHandler : AuthorizationMessageHandler
{
    public CloudCareApiHandler(
        IAccessTokenProvider provider, 
        NavigationManager navigation, 
        IConfiguration config) 
        : base(provider, navigation)
    {
        // get the API base URL from configuration
        var apiBaseUrl = config["api:BaseUrl"];

        //  Configure the handler to attach tokens for the API calls
        ConfigureHandler(
            authorizedUrls: new[] { apiBaseUrl },
            scopes: new[] { "openid", "profile", "email" }
        );
    }
}