using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CloudCare.Web;
using CloudCare.Web.Handlers;
using CloudCare.Web.Services;
using CloudCare.Web.Services.ExpenseTracker;
using CloudCare.Web.Services.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["api:BaseUrl"];
var auth0Audience = builder.Configuration["auth0:Audience"];



//  Register the (The Handler)
// This class contains the logic to attach the Access Token.
builder.Services.AddScoped<CloudCareApiHandler>();

//register http client with our api base url and handler
builder.Services.AddHttpClient("CloudCareApi", client => 
        client.BaseAddress = new Uri(builder.Configuration["api:BaseUrl"]))
    .AddHttpMessageHandler<CloudCareApiHandler>(); //this intercepts requests to add the token

//           This line says "Whenever ANYONE asks for 'HttpClient', do NOT give a blank one.
//              Instead, go to the Factory, create the specific 'CloudCareApi' client 
//           (which has the token handler), and give them that one."
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("CloudCareApi"));




builder.Services.AddScoped<ExpenseService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<PaymentMethodService>();
builder.Services.AddScoped<VendorService>();
builder.Services.AddScoped<UserProfileStateService>();
builder.Services.AddSingleton<ExpenseStateService>();




// ============================================================================
// AUTHENTICATION SETUP
// ============================================================================
builder.Services.AddOidcAuthentication(options =>
{
    // Configure your authentication provider options here.
    builder.Configuration.Bind("auth0", options.ProviderOptions);
    
    // It's important to specify authentication flow we are using is the OAuth and OIDC. 
    options.ProviderOptions.ResponseType = "code";
    
    // CRITICAL: You must add the audience here so the token works for your API
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", auth0Audience);

    // We can specify additional scopes if needed
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("email");
});


//add mudblazor services
builder.Services.AddMudServices();


await builder.Build().RunAsync();