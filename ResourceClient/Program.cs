using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Portal.SDK.ServiceClient;

namespace ResourceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            UseAuthorizationCode();
            //ValidateAccessToken();
            //AccessResource();
            Console.ReadLine();
        }

        static void UseAuthorizationCode()
        {
            var result = OauthServiceClient.GetAccessToken(clientId: "NDJjNWVlZmEtNmUxNC00ZDgwLWE4MWYtNmMyMGVmZGFiYzk5",
                clientSecret: "ZTFjNDZkNzktNjkzNC00ZWNlLWJkYmYtZmU4YmRjYTUyMTNkLWU3ZjAxZjliLTE5YTAtNGU3OS04MzRkLWQ4ZGEwZTliNGQ4ZA==",
                redirectUri: "http://www.wish.com/back", 
                grantType: "authorization_code",
                code: "NmI4ZmJiMDYtOTM4NS00NGFkLWJiYTktMTEwNTg0ZWY3ZDJj", refreshToken: "");
            Console.WriteLine("Get token by authcode success.");
            Console.ReadLine();
        }

        static void ValidateAccessToken()
        {
            var result = OauthServiceClient.ValidateToken(token: "YTg0NmRjM2EtMGIzZS00N2M0LTgxMjEtMGJiNTk3NDk0Mzk0",
                apiCode: "Get_Inventory");
            //Console.WriteLine(result.IsPassed);
            Console.WriteLine("validate access token success.");
            Console.ReadLine();
        }

        static void AccessResource()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "NTI2YTA4ODAtNjQ4Yy00YzM2LWIwOTAtY2Y1MTUyNDEyMGU11");
            HttpResponseMessage response = client.GetAsync("http://localhost:3722/api/demo").Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine("http 访问成功");
                Console.WriteLine(response.Content.ReadAsAsync<string>().Result);
            }
            else
            {
                Console.WriteLine("认证失败");
                var challenge = response.Headers.WwwAuthenticate.FirstOrDefault();
                if (challenge != null)
                {
                    Console.WriteLine(Encoding.UTF8.GetString(Convert.FromBase64String(challenge.Parameter)));
                }
            }
        }
    }
}
