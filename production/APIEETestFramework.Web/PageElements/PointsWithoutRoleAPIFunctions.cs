namespace UiIntegrationTest.PageElements
{
    using TestCommonUtils;
    using UiIntegrationTest.ElementRepo;
    using Newtonsoft.Json.Linq;
    using RestSharp;
    using System;
    using System.Net;

    public class PointsWithoutRole
    {
        private readonly CommonAPIFunctions _commonAPIFunctions;
        private readonly string token;
        private readonly EomApiPoco _eomApiPoco;
        public PointsWithoutRole(CommonAPIFunctions commonAPIFunctions, EomApiPoco eomApiPoco)
        {
            _commonAPIFunctions = commonAPIFunctions;
            token = _commonAPIFunctions.GetUserToken();
            _eomApiPoco = eomApiPoco;
        }

        public void CreatePoint(string name,string unit,string pointId,string label,string desc)
        {   
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var client = new RestClient(XmlHelper.GetKey("mutation"));
            var request = _commonAPIFunctions.RequestBuilder(token);
            request.AddJsonBody(new {query= "mutation{\n  createPoint(point: {\n  name:\""+name+"\"\n  unit:\""+unit+"\"\n  pointId:\""+ pointId+"\"   \n  labels:[{\n    value:\""+label+"\"\n    language:\"EN\"\n  }]\n  descriptions:[{\n  value:\""+desc+"\"\n  language:\"EN\"\n  }]\n  })\n}" });
            _eomApiPoco.response = client.Execute(request);
        }
        public void StorePoint()
        {
            dynamic jObject = JObject.Parse(_eomApiPoco.response.Content);
            var pointId = jObject.data.createPoint;
            EomApiPoco.pointGUID.Add(pointId.ToString());
            foreach (var value in EomApiPoco.pointGUID)
            {
                Console.WriteLine(value.ToString());
            }
        }
        public void readponitguids()
        {
            foreach (var value in EomApiPoco.pointGUID)
            {
                Console.WriteLine(value.ToString());
            }
        }
    }
}