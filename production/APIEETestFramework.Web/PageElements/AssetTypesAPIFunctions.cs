namespace UiIntegrationTest.PageElements
{
    using TestCommonUtils;
    using UiIntegrationTest.ElementRepo;
    using Newtonsoft.Json.Linq;
    using RestSharp;
    using System;
    using System.Net;

    public class AssetTypesAPIFunctions
    {
        private readonly CommonAPIFunctions _commonAPIFunctions;
        private readonly string token;
        private readonly EomApiPoco _eomApiPoco;
        public AssetTypesAPIFunctions(CommonAPIFunctions commonAPIFunctions, EomApiPoco eomApiPoco)
        {
            _commonAPIFunctions = commonAPIFunctions;
            token = _commonAPIFunctions.GetUserToken();
            _eomApiPoco = eomApiPoco;
        }
        public void GetAssetTypes()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var client = new RestClient(XmlHelper.GetKey("read"));
            var request = _commonAPIFunctions.RequestBuilder(token);
            request.AddJsonBody(new {query="query{assetTypes(language:\"en\") {description label id roles(language:\"en\") {description label id}}}"});

            _eomApiPoco.response = client.Execute(request);
        }

        public void CreateEnum()
        {   
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var client = new RestClient(XmlHelper.GetKey("mutation"));
            var request = _commonAPIFunctions.RequestBuilder(token);
            request.AddJsonBody(new {query= "mutation{\n  createEnum(enum: {\n enumMaps:[{\n    from:\"0\"\n    to:\"OFF\"\n  }\n{\n    from:\"1\"\n    to:\"ON\"\n  }\n{\n    from:\"2\"\n    to:\"Auto\"\n  }]\n\n  labels:[{\n    value:\"Enum3states\"\n    language:\"EN\"\n  }]\n})\n}"});
            _eomApiPoco.response = client.Execute(request);
        }

    }
}