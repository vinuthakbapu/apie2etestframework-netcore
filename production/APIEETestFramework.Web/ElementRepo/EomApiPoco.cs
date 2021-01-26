namespace UiIntegrationTest.ElementRepo
{
    using RestSharp;
    using System;
    using System.Collections.Generic;

    public class EomApiPoco
    {
        public IRestResponse response { get; set; }

        public static List<String> pointGUID = new List<String>();

    }
}