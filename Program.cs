using System;
using VRChat.API.Api;
using VRChat.API.Client;
using VRChat.API.Model;
using System.IO;
using System.Text.Json;

public class VRCAuthInfo {
    public string Username  { get; set;  } = string.Empty;
    public string Password  { get; set;  } = string.Empty;
    public string UserAgent { get; set;  } = string.Empty;

}


class LambdaApp
{
    static void Main(string[] args) 
    {
        try
        {

            string jsonStr = System.IO.File.ReadAllText("credentials.json");
            VRCAuthInfo authInfo = JsonSerializer.Deserialize<VRCAuthInfo>(jsonStr)!;
            Configuration config = new Configuration();

            if (authInfo.Username != string.Empty && authInfo.Password != string.Empty && authInfo.UserAgent != string.Empty)
            {
                config.Username = authInfo.Username;
                config.Password = authInfo.Password;
                config.UserAgent = authInfo.UserAgent;
            }

            // Create instances of API's we'll need
            AuthenticationApi AuthApi = new AuthenticationApi(config);
            UsersApi UserApi = new UsersApi(config);
            WorldsApi WorldApi = new WorldsApi(config);


            CurrentUser CurrentUser = AuthApi.GetCurrentUser();

            if (CurrentUser == null)
            {
                //if it's null it's most likely because of 2 factor so lets ask for that
                Console.WriteLine("Enter 2FA: ");
                var twoAuthCode = Console.ReadLine();
                Verify2FAResult result = AuthApi.Verify2FA(new TwoFactorAuthCode(twoAuthCode));
                AuthApi = new AuthenticationApi(config);
                CurrentUser = AuthApi.GetCurrentUser();
            }
            Console.WriteLine("Logged in as: {0}, using avatar: {1}", CurrentUser.DisplayName, CurrentUser.CurrentAvatar);

        }
        catch (ApiException e)
        {
            Console.WriteLine("Exception when calling API: {0}", e.Message);
            Console.WriteLine("Status Code: {0}", e.ErrorCode);
            Console.WriteLine(e.ToString());
        }
}
}