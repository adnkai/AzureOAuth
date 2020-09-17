using System;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Configuration;

namespace DeviceCodeAuth
{
    class Program
    {
        static IConfidentialClientApplication app;
        static IConfiguration Configuration;
        const string mail = "kai.roth@adn.de";
        static async Task Main(string[] args)
        {

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(@"C:\Users\Kai.Roth\Documents\Workshops\OAuth2.0\AzureOAuth\DeviceCodeAuth\appsettings.json");
            Configuration = configurationBuilder.Build();
            
            var clientID = "";
            var tenantID = "";
            var clientSecret = "";

            app = ConfidentialClientApplicationBuilder.Create(clientID)
                .WithClientSecret(clientSecret)
                .WithAuthority("https://login.microsoft.com/{0}", tenantID)
                .Build();
            var scopes = new [] {"api://AvaLAuth/.default"};
            AuthenticationResult result = null;
            try
            {
                result = await app.AcquireTokenForClient(scopes)
                                .ExecuteAsync();
                Console.WriteLine(result.AccessToken);
                
                ValidateToken(result.AccessToken, result.TenantId, clientID);

                Console.WriteLine(result.IdToken);              
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

        private static void ValidateToken(String _token, string _tenantID, string _audience) {  
            string token = _token;  
            string myTenant = _tenantID;  
            var myAudience = _audience;  
            var myIssuer = String.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/{0}/v2.0", myTenant);  
            var mySecret = "";  
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));              
            var stsDiscoveryEndpoint = String.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/{0}/.well-known/openid-configuration", myTenant);  
            
            var tokenHandler = new JwtSecurityTokenHandler();  
  
            var validationParameters = new TokenValidationParameters
            {  
                ValidAudience = myAudience,  
                ValidIssuer = myIssuer,  
                ValidateLifetime = false,  
                IssuerSigningKey = mySecurityKey  
            };  
  
            var validatedToken = (SecurityToken)new JwtSecurityToken();  
  
            // Throws an Exception as the token is invalid (expired, invalid-formatted, etc.)  
            tokenHandler.ValidateToken(token, validationParameters, out validatedToken);  
            Console.WriteLine(validatedToken);  
            Console.ReadLine();  
        } 

    }
}
