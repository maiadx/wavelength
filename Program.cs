using System;
using VRChat.API.Api;
using VRChat.API.Client;
using VRChat.API.Model;

public class MyAuthInfo {
    public string Username  { get; set;  }
    public string Password  { get; set;  }
    public string UserAgent { get; set;  }
}

string jsonStr = File.ReadAllText("credentials.json");
var creds = JsonSerializer.Deserialize<MyAuthInfo>(jsonString);



// Authentication credentials
Configuration Config = new Configuration();


// Create instances of API's we'll need
AuthenticationApi AuthApi = new AuthenticationApi(Config);
UsersApi UserApi = new UsersApi(Config);
WorldsApi WorldApi = new WorldsApi(Config);

try
{
    CurrentUser CurrentUser = AuthApi.GetCurrentUser();

    if (CurrentUser == null)
    {
        //if it's null it's most likely because of 2 factor so lets ask for that
        Console.WriteLine("Enter 2FA: ");
        var twoAuthCode = Console.ReadLine();
        Verify2FAResult result = AuthApi.Verify2FA(new TwoFactorAuthCode(twoAuthCode));
        AuthApi = new AuthenticationApi(Config);
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