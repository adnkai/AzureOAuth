# AzureOAuth
OAuth Examples with MSAL


# Tutorial
Eine neue WebApp mit Multi-Authentisierung erstellen:
> dotnet new webapp -o AppName -au Individual -uld

Die App starten und den `Register` Button klicken. Es wird ein Fehler geworfen, die Datenbank muss migriert werden.

Nach der Migration können wir den Code anpassen, um unsere Authentisierungsoptionen hinzuzufügen.

https://docs.microsoft.com/de-de/aspnet/core/security/authentication/social/?view=aspnetcore-3.1&tabs=visual-studio-code


Für AzureAD muss in der App Registrierung als RedirectUri
> https://localhost:IIS-SSL-Port/signin-oidc

Als Logout-URI
> https://localhost:IIS-SSL-Port/signout-oidc
eingetragen werden.

Im Code müssen wir außerdem das Cookie-Scheme anpassen:

> .AddAzureAD(options => {
>                        Configuration.Bind("Authentication:AzureAd", options);
>                        options.CookieSchemeName = IdentityConstants.ExternalScheme;
>                    })


Microsoft-Sign-In
> https://docs.microsoft.com/de-de/aspnet/core/security/authentication/social/microsoft-logins?view=aspnetcore-3.1

Login-Einschränkungen nach Schema
> https://docs.microsoft.com/de-de/aspnet/core/security/authorization/limitingidentitybyscheme?view=aspnetcore-3.1

Web-App Sign-In Konfigurationsdatei
> https://docs.microsoft.com/de-de/azure/active-directory/develop/scenario-web-app-sign-user-app-configuration?tabs=aspnetcore

Microsoft Identity Platform
> https://docs.microsoft.com/de-de/azure/active-directory/develop/sample-v2-code

Microsoft Authentication Flows
> https://docs.microsoft.com/de-de/azure/active-directory/develop/authentication-flows-app-scenarios#scenarios-and-supported-authentication-flows

Single-Page Application
> https://docs.microsoft.com/de-de/azure/active-directory/develop/scenario-spa-overview

Labs
> https://docs.microsoft.com/en-us/samples/azure-samples/active-directory-aspnetcore-webapp-openidconnect-v2/enable-webapp-signin/

