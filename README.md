# Saml2.Authentication.Core
A SAML 2.0 authentication middleware for ASP.NET Core

This project is a fork of the [OIOSAML.Net](https://www.digitaliser.dk/resource/3849744) implementation of SAML 2.0 framework from [digitaliser.dk](https://www.digitaliser.dk/). It has been ported and modified to support ASP.NET Core with all dependencies to ASP.NET removed.

## Features
Supports the following SAML 2.0 features for Web Browser SSO and Single Logout profiles
  - [x]  HTTP Redirect Binding <br/>
         SP Redirect Request; IdP POST/Redirect Response
  - [x]  HTTP Artifact Binding <br/>
         SP Redirect Request; IdP Redirect Artifact Response
  - [x] SP-Initiated Single Logout with Multiple SPs
  - [ ] HTTP POST Binding <br/>
  - [ ] IDP-Initiated Single Logout
  
## Configuration
### Startup
```
  // This method gets called by the runtime. Use this method to add services to the container.
  public void ConfigureServices(IServiceCollection services)
  {
      services.AddScoped<IUserClaimsPrincipalFactory<TUser>, DemoWebAppClaimsPrincipalFactory>();		
      services.Configure<Saml2Configuration>(Configuration.GetSection("Saml2"));

      services.AddSaml();

      // Single idp
      services.AddAuthentication()
          .AddCookie("saml2.cookies", options =>
          {
              options.Cookie.HttpOnly = true;
              options.Cookie.SameSite = SameSiteMode.None;
              options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
          })
          .AddSaml("saml2", "saml2", options =>
          {
              options.SignInScheme = "saml2.cookies";
              options.IdentityProviderName = "stubidp.sustainsys";
          });
          
      // Multiple idps
       services.AddAuthentication()
          .AddCookie("saml2.idp1.cookies", options =>
          {
              options.Cookie.HttpOnly = true;
              options.Cookie.SameSite = SameSiteMode.None;
              options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
          })
          .AddCookie("saml2.idp2.cookies", options =>
          {
              options.Cookie.HttpOnly = true;
              options.Cookie.SameSite = SameSiteMode.None;
              options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
          })
          .AddSaml("saml2.idp1", "saml2.idp1", options =>
          {
              options.SignInScheme = "saml2.idp1.cookies";
              options.IdentityProviderName = "idp1";
          })
          .AddSaml("saml2.idp2", "saml2.idp2", options =>
           {
               options.SignInScheme = "saml2.idp2.cookies";
               options.IdentityProviderName = "idp2";
           });

      services.AddMvc();
  }
```
### appsettings.json
```
// Single idp
"Saml2": {
    "ServiceProviderConfiguration": {
      "EntityId": "Id of the sp",
      "Name": "name",
      "AssertionConsumerServiceUrl": "AssertionConsumerService",
      "SingleLogoutResponseServiceUrl": "SingleLogoutService",
      "OmitAssertionSignatureCheck": true, // check or not for valid idp's signature in AuthnResponse
      "Certificate": {
        "Thumbprint": "sp's certificate",
      }
    },
    "IdentityProviderConfiguration": [
      {
        "EntityId": "Id of the SAML 2.0 idp",
        "Name": "Name of the SAML 2.0 idp",
        "ForceAuth": "false",
        "IsPassive": "false",
        "SingleSignOnService": "idp's sso service endpoint",
        "SingleSignOutService": "idp's slo service endpoint",
        "ArtifactResolveService": "idp's artifact resolve service endpoint",
        "Certificate": {
          "Thumbprint": "idp's certificate",
        }
      }
    ]
  }
  
  // Multiple idps
  "Saml2": {
   "ServiceProviderConfiguration": {
      "EntityId": "Id of the sp",
      "Name": "name",
      "AssertionConsumerServiceUrl": "AssertionConsumerService",
      "SingleLogoutResponseServiceUrl": "SingleLogoutService",
      "OmitAssertionSignatureCheck": true, // check or not for valid idp's signature in AuthnResponse
      "Certificate": {
        "Thumbprint": "sp's certificate",
      }
    },
    "IdentityProviderConfiguration": [
      {
        "EntityId": "idp1",
        "Name": "name of idp1",
        "ForceAuth": "false",
        "IsPassive": "false",
        "SingleSignOnService": "idp1's sso service endpoint",
        "SingleSignOutService": "idp1's slo service endpoint",
        "ArtifactResolveService": "idp1's artifact resolve service endpoint",
        "Certificate": {
          "Thumbprint": "idp1's certificate",
        }
      },
      {
        "EntityId": "idp2",
        "Name": "name of idp2",
        "ForceAuth": "false",
        "IsPassive": "false",
        "SingleSignOnService": "idp2's sso service endpoint",
        "SingleSignOutService": "idp2's slo service endpoint",
        "ArtifactResolveService": "idp2's artifact resolve service endpoint",
        "Certificate": {
          "Thumbprint": "idp2's certificate",
        }
      }
    ]
  }
```
### ClaimsPrincipalFactory
The SessionIndex and Subject claims are required for SLO. These needs to be stored and availed during logout.
This example keeps all the claims from the idp in session cookie if using [Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?tabs=visual-studio%2Caspnetcore2x)

```
  public class DemoWebAppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
  {
      private readonly IHttpContextAccessor _httpContextAccessor;

      public DemoWebAppClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
          IOptions<IdentityOptions> optionsAccessor, IHttpContextAccessor httpContextAccessor) : base(userManager,
          optionsAccessor)
      {
          _httpContextAccessor = httpContextAccessor;
      }

      protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var signInManager =
                (SignInManager<ApplicationUser>)Context.RequestServices.GetService(
                    typeof(SignInManager<ApplicationUser>));

            var claims = new List<Claim>();
            var authenticationSchemes = await signInManager.GetExternalAuthenticationSchemesAsync();
            foreach (var scheme in authenticationSchemes)
            {
                var authenticateResult = await Context.AuthenticateAsync(scheme.Name);
                if (!authenticateResult.Succeeded)
                {
                    continue;
                }

                var sessionIndex = authenticateResult.Principal.Claims.First(c => c.Type == Saml2ClaimTypes.SessionIndex);
                var saml2Subject = authenticateResult.Principal.Claims.First(c => c.Type == Saml2ClaimTypes.Subject);
                claims.Add(sessionIndex);
                claims.Add(saml2Subject);
            }

            var claimsIdentity = await base.GenerateClaimsAsync(user);
            claimsIdentity.AddClaims(claims); //Add external claims to cookie. The SessionIndex and Subject are required for SLO
            return claimsIdentity;
        }
  }
```
