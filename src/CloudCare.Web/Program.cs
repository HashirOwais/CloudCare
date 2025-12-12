using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CloudCare.Web;
using CloudCare.Web.Extensions;
using CloudCare.Web.Handlers;
using CloudCare.Web.Services;
using CloudCare.Web.Services.ExpenseTracker;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["api:BaseUrl"];
var auth0Audience = builder.Configuration["auth0:Audience"];

// ============================================================================
// HTTP CLIENT & SERVICE REGISTRATION (Option 3: Typed Clients)
// ============================================================================

// 1. Register the Custom Handler 
// This reads the config internally and manages the Token attachment logic
builder.Services.AddScoped<CloudCareApiHandler>();

// 2. Register Expense Service
// This automatically:
//    a. Creates the ExpenseService
//    b. Configures its internal HttpClient with the BaseAddress
//    c. Wires up the CloudCareApiHandler to attach tokens securely

builder.Services.AddApiClient<ExpenseService>(apiBaseUrl!);
builder.Services.AddApiClient<PaymentMethodService>(apiBaseUrl!);
builder.Services.AddApiClient<CategoryService>(apiBaseUrl!);
builder.Services.AddApiClient<VendorService>(apiBaseUrl!);
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