# AzureOAuth
OAuth Examples with MSAL


# Tutorial
Eine neue WebApp mit Multi-Authentisierung erstellen:
> dotnet new webapp -o AppName -au Individual -uld

Die App starten und den `Register` Button klicken. Es wird ein Fehler geworfen, die Datenbank muss migriert werden.

Nach der Migration können wir den Code anpassen, um unsere Authentisierungsoptionen hinzuzufügen.

https://docs.microsoft.com/de-de/aspnet/core/security/authentication/social/?view=aspnetcore-3.1&tabs=visual-studio-code


Für AzureAD muss in der App Registrierung als RedirectUri
> https://localhost:<IIS-SSL-Port>/signin-oidc
Als Logout-URI
> https://localhost:<IIS-SSL-Port>/signout-oidc
eingetragen werden.

Im Code müssen wir außerdem das Cookie-Scheme anpassen:

> .AddAzureAD(options => {
>                        Configuration.Bind("Authentication:AzureAd", options);
>                        options.CookieSchemeName = IdentityConstants.ExternalScheme;
>                    })
