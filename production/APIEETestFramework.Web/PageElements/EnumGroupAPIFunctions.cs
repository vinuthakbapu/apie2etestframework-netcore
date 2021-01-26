namespace UiIntegrationTest.PageElements
{
    using TestCommonUtils;
    using UiIntegrationTest.ElementRepo;
    using RestSharp;
    using System;
    using System.Net;

    public class EnumGroupAPIFunctions
    {
        private readonly CommonAPIFunctions _commonAPIFunctions;
        private readonly string token;
        private readonly EomApiPoco _eomApiPoco;
        public EnumGroupAPIFunctions(CommonAPIFunctions commonAPIFunctions, EomApiPoco eomApiPoco)
        {
            _commonAPIFunctions = commonAPIFunctions;
            token = _commonAPIFunctions.GetUserToken();
            _eomApiPoco = eomApiPoco;
        }

        public void AssignEnum(string pointId)
        {   
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var client = new RestClient(XmlHelper.GetKey("mutation"));
            var request = _commonAPIFunctions.RequestBuilder(token);
            Console.WriteLine("EomApiPoco.pointGUID[Int32.Parse(pointId)]-->{0}", EomApiPoco.pointGUID[Int32.Parse(pointId)]);
            string requestPayload = "mutation{ assignEnumsToPoint(point: \"" + EomApiPoco.pointGUID[Int32.Parse(pointId)] + "\", assignEnums:[\"" + XmlHelper.GetKey("EnumID") + "\"])}";
            Console.WriteLine("requestPayload-->{0}", requestPayload);
            request.AddJsonBody(new { query = "mutation{ assignEnumsToPoint(point: \""+ EomApiPoco.pointGUID[Int32.Parse(pointId)] +"\", assignEnums:[\"" + XmlHelper.GetKey("EnumID") +"\"])}" });
            _eomApiPoco.response = client.Execute(request);
        }
        public void UpdatePoints(string pointId)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var client = new RestClient(XmlHelper.GetKey("mutation"));
            var request = _commonAPIFunctions.RequestBuilder(token);
            string requestPayload = "mutation{ updatePoint(point:{ id:\"" + EomApiPoco.pointGUID[Int32.Parse(pointId)] + "\" pointId:\"AHU1_ReturnAirTemperatureSetpoint\" labels:[{value:\"Updated\" language:\"EN\"}] unit:\"°C\" })} ";
            Console.WriteLine("requestPayload-->{0}", requestPayload);
            request.AddJsonBody(new { query = "mutation{updatePoint(point:{id:\"" + EomApiPoco.pointGUID[Int32.Parse(pointId)] + "\" pointId:\"AHU1_ReturnAirTemperatureSetpoint\" labels:[{value:\"Updated\" language:\"EN\"}] unit:\"°C\" })} " });
            _eomApiPoco.response = client.Execute(request);
        }
        public void GetPoints()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var client = new RestClient(XmlHelper.GetKey("read"));
            var request = _commonAPIFunctions.RequestBuilder(token);
            string requestPayload = "query{points(language: \"en\"){ databaseName description id label pointId serverAddress unit}}";
            Console.WriteLine("requestPayload-->{0}", requestPayload);
            request.AddJsonBody(new { query="query{points(language: \"en\"){ databaseName description id label pointId serverAddress unit}}" });
            _eomApiPoco.response = client.Execute(request);
        }
        public void GetPointsWithID(string pointId)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var client = new RestClient(XmlHelper.GetKey("read"));
            var request = _commonAPIFunctions.RequestBuilder(token);
            string requestPayload = "query{point(id: \"" + EomApiPoco.pointGUID[Int32.Parse(pointId)] + "\"){databaseName description id label pointId serverAddress unit role{id label    description aspects{name value}}}}";
            Console.WriteLine("requestPayload-->{0}", requestPayload);
            request.AddJsonBody(new { query= "query{point(id: \"" + EomApiPoco.pointGUID[Int32.Parse(pointId)] + "\"){databaseName description id label pointId serverAddress unit role{id label    description aspects{name value}}}}" });
            _eomApiPoco.response = client.Execute(request);
        }
    }
}