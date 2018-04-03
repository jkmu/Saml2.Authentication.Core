# Saml2.Authentication.Core
A SAML 2.0 middleware for ASP.NET Core
NB: WORK IN PROGRESS!

This project is a fork of the [OIOSAML.Net](https://www.digitaliser.dk/resource/3849744) implementation of SAML2 framework from [digitaliser.dk](https://www.digitaliser.dk/). It has been ported and modified to support ASP.NET Core with all dependencies to ASP.NET removed.

## Features
Supports the following SAML2 features for Web Browser SSO and Single Logout profiles
  - [x]  HTTP Redirect Binding <br/>
         SP Redirect Request; IdP POST/Redirect Response
  - [x]  HTTP Artifact Binding <br/>
         SP Redirect Request; IdP Redirect Artifact Response (Not fully tested)
  - [x] SP-Initiated Single Logout with Multiple SPs
  - [ ] HTTP POST Binding <br/>
  - [ ] IDP-Initiated Single Logout
  
## Configuration
### Startup
```
  // This method gets called by the runtime. Use this method to add services to the container.
  public void ConfigureServices(IServiceCollection services)
  {
      services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, DemoWebAppClaimsPrincipalFactory>();		
      services.Configure<Saml2Configuration>(Configuration.GetSection("Saml2"));

      services.AddSaml();
      services.AddSigningCertificates(
          Configuration["Saml2:ServiceProviderConfiguration:SigningCertificateThumprint"],
          Configuration["Saml2:IdentityProviderConfiguration:SigningCertificateThumprint"]);

      services.AddAuthentication()
          .AddCookie()
          .AddSaml(options => { options.SignInScheme = "SignInSchemeName"; });

      services.AddMvc();
  }
```
### appsettings.json
```
"Saml2": {
    "ForceAuth": "false",
    "IsPassive": "false",
    "IdentityProviderConfiguration": {
      "EntityId": "EntityId of idp",
      "Name": "Name of idp",
      "SigningCertificateThumprint": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
      "SingleSignOnService": "https://XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
      "SingleSignOutService": "https://XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
      "ProtocolBinding": "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect"
    },
    "ServiceProviderConfiguration": {
      "EntityId": "EntityId of sp",
      "Name": "Name of sp",
      "SigningCertificateThumprint": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
    }
  }
```
### More
