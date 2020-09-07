using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Threading.Tasks;


namespace DeviceCodeAuth
{
    class Program
    {
        static IConfidentialClientApplication app;
        static IConfiguration Configuration;
        static async Task Main(string[] args)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(@"C:\Users\Kai.Roth\Documents\Workshops\OAuth2.0\AzureOAuth\DeviceCodeAuth\appsettings.json");
            Configuration = configurationBuilder.Build();
            Console.WriteLine(Configuration["ClientId"]);

            app = ConfidentialClientApplicationBuilder.Create(Configuration["ClientId"])
                .WithClientSecret(Configuration["ClientSecret"])
                .WithAuthority(new Uri(Configuration["Authority"]))
                .Build();
            var scopes = new [] {Configuration["ResourceId"]};
            AuthenticationResult result = null;
            try
            {
                result = await app.AcquireTokenForClient(scopes)
                                .ExecuteAsync();
                Console.WriteLine(result.AccessToken);
            }
            catch (MsalUiRequiredException ex)
            {
                throw(ex);
                // The application doesn't have sufficient permissions.
                // - Did you declare enough app permissions during app creation?
                // - Did the tenant admin grant permissions to the application?
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                // Invalid scope. The scope has to be in the form "https://resourceurl/.default"
                // Mitigation: Change the scope to be as expected.
            } catch (Exception e) {
                throw(e);
            }
        }

    }
}
