# MoreApp C# Starter Kit

An example project to help MoreApp partners with their first usage of the API.

## Run

Open the solution with Visual Studio and run the application.

## Example code

Check out the file `CSharpStarterKit/Program.cs` to see interaction with the API.


It sets up an client with authorization settings (using the loaded properties file). The client can be used to make an authorized call to the MoreApp API. The example code will fetch all customers for the authorized partner.

```
string url = baseUrl + path;
OAuthRequest client = new OAuthRequest
{
    Method = "GET",
    Type = OAuthRequestType.RequestToken,
    SignatureMethod = OAuthSignatureMethod.HmacSha1,
    ConsumerKey = settings.consumerKey,
    ConsumerSecret = GetSha1(settings.consumerSecret, settings.salt),
    RequestUrl = url
};

string auth = client.GetAuthorizationHeader();
WebClient c = new WebClient();
c.Headers.Add("Authorization", auth);
c.BaseAddress = baseUrl;
return c.DownloadString(path);
```

After the API call the result will be printed to the console

```
Console.WriteLine("The customers are:");
List<Dictionary<String, String>> customers = JsonConvert.DeserializeObject<List<Dictionary<String, String>>>(stringResponse);

customers.ForEach(delegate(Dictionary<String, String> customer) {
    Console.WriteLine(" - " + customer["name"]);
}); 
```

## Changing the example for your own usage

To use the example for your own usage change the file `CSharpStarterKit/Settings.settings`.

- The `endpoint` should be `https://api.moreapp.com/api/v1.0`. For the example we use the MoreApp develop environment. Please do not use this.  
- The `salt` property should be replaced with the salt that you can acquire in de developer portal under FAQ. 
- The `consumerKey` and `consumerSecret` properties should be changed into the correct partner credentials.

