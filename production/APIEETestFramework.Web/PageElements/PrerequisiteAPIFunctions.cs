namespace UiIntegrationTest.PageElements
{
    using TestCommonUtils;
    using UiIntegrationTest.ElementRepo;
    using Newtonsoft.Json.Linq;
    using RestSharp;
    using System;
    using System.Net;
    using System.Threading;

    public class PrerequisiteAPIFunctions
    {
        private readonly CommonAPIFunctions _commonAPIFunctions;
        private readonly string token;
        private readonly EomApiPoco _eomApiPoco;
        public PrerequisiteAPIFunctions(CommonAPIFunctions commonAPIFunctions, EomApiPoco eomApiPoco)
        {
            _commonAPIFunctions = commonAPIFunctions;
            //token = _commonAPIFunctions.GetUserToken();
            _eomApiPoco = eomApiPoco;
        }
        public void CreateCustomer()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var client = new RestClient(XmlHelper.GetKey("mutation"));
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Authorization", $"Bearer {token.ToString()}");
            request.AddJsonBody(new { query = "mutation{ createCustomer }" });
            _eomApiPoco.response = client.Execute(request);
            dynamic jObject = JObject.Parse(_eomApiPoco.response.Content);
            Console.WriteLine("Success-->" + jObject.ToString());
        }
        
        public void LoadSOM()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var client = new RestClient(XmlHelper.GetKey("mutation"));
            var request = _commonAPIFunctions.RequestBuilder(token);
            request.AddJsonBody(new {query= "mutation{loadSOM(customerId:\""+ XmlHelper.GetKey("CustomerID") + "\", customerType:\"hce-cb\")}" });

            _eomApiPoco.response = client.Execute(request);
        }
        public void CheckSOM()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var client = new RestClient(XmlHelper.GetKey("read"));
            var request = _commonAPIFunctions.RequestBuilder(token);
            request.AddJsonBody(new { query = "query{getCustomerStatus(customerId:\"" + XmlHelper.GetKey("CustomerID") + "\")}" });
        }
        public void ValidateCheckSOM(string status)
        {
            string getCustomerStatus;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var client = new RestClient(XmlHelper.GetKey("read"));
            var request = _commonAPIFunctions.RequestBuilder(token);
            request.AddJsonBody(new { query = "query{getCustomerStatus(customerId:\"" + XmlHelper.GetKey("CustomerID") + "\")}" });
            var attempt = 0;
            while (attempt < 10)
            {
                _eomApiPoco.response = client.Execute(request);
                dynamic jObject = JObject.Parse(_eomApiPoco.response.Content);
                Console.WriteLine("Status code--{0}", _eomApiPoco.response.StatusCode);
                Console.WriteLine("HttpStatusCode.OK--{0}", HttpStatusCode.OK);
                Console.WriteLine("CheckSOM: response -->{0}", jObject.ToString());
                getCustomerStatus = jObject.data.getCustomerStatus.ToString();
                if (_eomApiPoco.response.StatusCode == HttpStatusCode.OK && getCustomerStatus == status)
                {
                    Console.WriteLine("CheckSOM: Message -->{0}", jObject.ToString());
                    break;
                }
                else if (_eomApiPoco.response.StatusCode == HttpStatusCode.OK && getCustomerStatus == "FAILED")
                {
                    //Assert.Fail("CheckSOM getCustomerStatus is Failed -->{0}", jObject.ToString());
                    break;
                }
                else if (_eomApiPoco.response.StatusCode != HttpStatusCode.OK)
                {
                    //Assert.Fail("CheckSOM getCustomerStatus is Failed -->{0}", jObject.ToString());
                    break;
                }
                Thread.Sleep(TimeSpan.FromSeconds(5));
                attempt++;
            }
            //while (getCustomerStatus != status);
        }
        public void DeleteCustomer()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var client = new RestClient(XmlHelper.GetKey("mutation"));
            var request = _commonAPIFunctions.RequestBuilder(token);
            request.AddJsonBody(new { query="mutation{deleteCustomer(customerId: \""+ XmlHelper.GetKey("CustomerID") + "\")}" });

            _eomApiPoco.response = client.Execute(request);
        }
    }
}