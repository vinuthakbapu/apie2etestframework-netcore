using TestCommonUtils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net.Http;
using System.Text;

namespace UiIntegrationTest.PageElements
{
    public class AdminPortalTokenGenerator
    {
        public static string GetUserToken()
        {
            using (HttpClient client = new HttpClient())
            {
                var tokenEndpoint = XmlHelper.GetKey("TokenEndpoint");
                var accept = "application/json";
                var adminResource = XmlHelper.GetKey("resource");

                client.DefaultRequestHeaders.Add("Accept", accept);
                string postBody = @"resource=" + adminResource + "&client_id=" + XmlHelper.GetKey("client_id") + " & grant_type=password&username=" + XmlHelper.GetKey("username") + "&password=" + XmlHelper.GetKey("password") + " & scope=openid";

                using (var response = client.PostAsync(tokenEndpoint, new StringContent(postBody, Encoding.UTF8, "application/x-www-form-urlencoded")).GetAwaiter().GetResult())
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonresult = JObject.Parse(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                        var token = (string)jsonresult["access_token"];
                        Console.WriteLine("token-->{0}", token.ToString());
                        return token;
                    }
                }
            }
            return null;
        }

       public static string CDTObtainToken()
        {
            var url = XmlHelper.GetKey("CDT_GlobalTokenEndpoint");

            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);

            request.AddParameter("client_id", XmlHelper.GetKey("CDT_appId"));
            request.AddParameter("client_secret", XmlHelper.GetKey("CDT_appKey"));
            request.AddParameter("resource", XmlHelper.GetKey("CDT_Resource"));
            request.AddParameter("grant_type", "client_credentials");

            IRestResponse response = client.Execute(request);

            var tokenResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
            string[] tokenRespnse = response.Content.Split(',');
            string[] value = tokenRespnse[6].Split(':');
            value[1] = value[1].Remove(value[1].Length - 1, 1);
            string ResponseCodeToken = value[1].Replace('"', ' ').Trim();
            return ResponseCodeToken;
        }
    }
}