using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Net;

using Newtonsoft.Json;
using OAuth;
using System.Security.Cryptography;

using System.Collections.Specialized;

namespace CSharpStarterKit
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load the properties
            Settings settings = Settings.Default;

            //Make Request
            string stringResponse = MakeRequest(settings, settings.endpoint, "/api/v1.0/customers");
            //Print the result
            Console.WriteLine("The customers are:");
            List<Dictionary<String, String>> customers = JsonConvert.DeserializeObject<List<Dictionary<String, String>>>(stringResponse);

            customers.ForEach(delegate(Dictionary<String, String> customer) {
                Console.WriteLine(" - " + customer["name"]);
            });

            Dictionary<String, String> data = new Dictionary<String, String>();
            data.Add("pageSize", "10");
            string postResponse = MakeRequest(settings, settings.endpoint, "/api/v1.0/customers/900002/applications/c900002a1/forms/6971171074bad4a29c09c78eb036b130VIEW_ZERO/registrations", "POST", data);
            Dictionary<String, Object> registrations = JsonConvert.DeserializeObject<Dictionary<String, Object>>(postResponse);
            Console.WriteLine("Total size: " + registrations["totalSize"]);

            Console.Write("Press any key...");
            Console.ReadLine();
        }

        private static string MakeRequest(Settings settings, string baseUrl, string path, string method = "GET", Dictionary<String, String> data = null)
        {
            string url = baseUrl + path;
            //create an instance of OAuthRequest with the appropriate properties
            OAuthRequest client = new OAuthRequest
            {
                Method = method,
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
            if (method == "GET")
            {
                return c.DownloadString(path);
            }
            else
            {
                c.Headers[HttpRequestHeader.ContentType] = "application/json";
                return c.UploadString(path, JsonConvert.SerializeObject(data));
            }
        }

        //Method to salt the password and hash the password using a SHA-1
        private static string GetSha1(string password, string salt)
        {
            string prefix = salt.Substring(0, (int)Math.Ceiling((double)salt.Length / 2));
            string postfix = salt.Substring((int)Math.Ceiling((double)salt.Length / 2), (int)Math.Floor((double)salt.Length / 2));
            string key = prefix + password + postfix;
            var data = Encoding.ASCII.GetBytes(key);
            var hashData = new SHA1Managed().ComputeHash(data);
            var hash = string.Empty;
            foreach (var b in hashData)
                hash += b.ToString("X2");
            return hash.ToLower();
        }
    }
}

