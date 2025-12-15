# Why we need Factories

- `CustomAccountFactory.cs`
  - Currently, when a user logs in auth0 send back a token. 
  - The token contains user information such as email, name, etc.
    - However, auth0 doesnt know about our application such it doesnt know has that person paid their subscrptions or have they setup a daycare profile, does that person even exist?
- So the point here is that auth0 doesnt care about who that user is as long as they authenticate 
- But we want to check if that user exists in our system and if not create them
- So if we look at the Blazor login process.:
  - once the `AuthenticationState` is set. There is no way where we can edit it, i.e add more properties to it
  - - So what we do is that the `AccountCliamsPrincipalFactory` is the only place where we can touch and modify the `ClaimsPrincipal` (user) before it gets set as the `AuthenticationState`
- So we create a custom factory that inherits from `AccountCliamsPrincipalFactory` and override the `CreateAsync` method
  - In this method we check if the user exists in our system
    - If they dont we redirect them to create a profile
    - if they do then we keep on moving 
    - we will add a propety in the user claims to indicate if the user has a profile or not
    - Without Factory: Token → Microsoft Default Factory → User is logged in. (App doesn't know if they are new).
    - With Custom Factory: Token → Your Factory → Calls API → Adds "New User" Tag → User is logged in. (App sees the tag and redirects them).

- Now we can totally bypass this by just checking after they login and redirecting them
  - but then it will cause flickering as the user will see the main page load and then get redirected (tried and it was not good )

- This is the main reason why we need factories in this method 

---
### Implementation Details: Custom User Registration Flow

Building upon the rationale outlined above, we've implemented the custom authentication flow as follows:

#### 1. CustomAccountFactory.cs Creation

*   **Why:** To intercept the authentication process right after Auth0 returns a token but *before* the application fully initializes the user's `AuthenticationState`. This is the ideal point to enrich the user's claims with application-specific data (like whether they have a profile in our local database) without causing UI flickering.
*   **How:**
    *   A `CustomAccountFactory.cs` file was created in `CloudCare.Web/Factories/`.
    *   This class inherits from `AccountClaimsPrincipalFactory<RemoteUserAccount>`.
    *   It injects the `UserService` to interact with our backend API.
    *   The `CreateUserAsync` method was overridden. Inside this method:
        *   The base `CreateUserAsync` is called to get the initial `ClaimsPrincipal`.
        *   If the user is authenticated, `UserService.UserExistsAsync()` is called to check for a local database profile.
        *   A new `Claim` named `"profile_exists"` is added to the user's identity. Its value is "true" if the user exists in our DB, and "false" otherwise. This claim now signals the application if a user needs to complete registration.

#### 2. CustomAccountFactory Registration in Program.cs

*   **Why:** To ensure our application uses the `CustomAccountFactory` instead of the default Blazor authentication factory.
*   **How:**
    *   `using CloudCare.Web.Factories;` was added to `CloudCare.Web/Program.cs`.
    *   The line `builder.Services.AddScoped<AccountClaimsPrincipalFactory<RemoteUserAccount>, CustomAccountFactory>();` was added after the `AddOidcAuthentication` call. This replaces the default factory with our custom implementation.

#### 3. Redirection Logic in App.razor

*   **Why:** To provide a seamless redirection experience for users who are authenticated but lack a local profile, guiding them directly to the registration page without first showing them other parts of the application or causing a "flicker."
*   **How:**
    *   `CloudCare.Web/App.razor` was modified to explicitly define the `<Authorized>`, `<NotAuthorized>`, and `<Authorizing>` templates within the `AuthorizeRouteView`.
    *   Inside the `<Authorized>` and `<NotAuthorized>` templates, logic was added to check for the `"profile_exists"` claim.
    *   If the user is authenticated and the `"profile_exists"` claim is "false", the `UserRegistration` component is rendered directly, ensuring they complete their profile before accessing other application features. Otherwise, the standard `RouteView` or "Not Authorized" message is displayed.

This comprehensive approach leverages the Blazor authentication pipeline to integrate a smooth user onboarding experience, preventing the issues associated with post-load checks and enriching user identity at the earliest possible stage.