using TestCommonUtils;
using UiIntegrationTest.ElementRepo;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace UiIntegrationTest.PageElements
{
    public class CommonAPIFunctions
    {
        private readonly EomApiPoco _eomApiPoco;
        public CommonAPIFunctions(EomApiPoco eomApiPoco)
        {
            _eomApiPoco = eomApiPoco;
        }
        public string GetUserToken()
        {
            using (HttpClient client = new HttpClient())
            {
                var tokenEndpoint = XmlHelper.GetKey("Authorization");
                var accept = "application/json";
                var adminResource = XmlHelper.GetKey("EOM_resource");

                client.DefaultRequestHeaders.Add("Accept", accept);
                string postBody = @"resource=" + adminResource + "&client_id=" + XmlHelper.GetKey("EOM_client_id") + " & grant_type=" + XmlHelper.GetKey("EOM_grant_type") + "&client_secret=" + XmlHelper.GetKey("EOM_client_secret");

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

        public RestRequest RequestBuilder(string token)
        {
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {token.ToString()}");
            if(XmlHelper.GetKey("CustomerID") == null || XmlHelper.GetKey("CustomerID") =="")
            {
                //Assert.Fail("Customer ID is null");
            }
            request.AddHeader("customerId", XmlHelper.GetKey("CustomerID"));
            return request;
        }
        public void ValidateResponse(string methodname)
        {
            string restResponse = _eomApiPoco.response.Content;
            Console.WriteLine("CreateCustomer: Message -->{0}", restResponse.ToString());
            Console.WriteLine("Status code--{0}", _eomApiPoco.response.StatusCode);
            Console.WriteLine("HttpStatusCode.OK--{0}", HttpStatusCode.OK);
            dynamic jObject = JObject.Parse(_eomApiPoco.response.Content);
            if (_eomApiPoco.response.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine(methodname + "Success-->" + jObject.ToString());
            }
            else
            {
                //Assert.Fail(methodname + " Failed -->" + jObject.ToString());
            }
        }
        public void StoreCustomerID()
        {
            dynamic jObject = JObject.Parse(_eomApiPoco.response.Content);
            var customerId = jObject.data.createCustomer;
            XmlHelper.SetKey("CustomerID", customerId.ToString());
        }
        public void StoreEnumID()
        {
            dynamic jObject = JObject.Parse(_eomApiPoco.response.Content);
            var enumId = jObject.data.createEnum;
            XmlHelper.SetKey("EnumID", enumId.ToString());
        }
    }
}