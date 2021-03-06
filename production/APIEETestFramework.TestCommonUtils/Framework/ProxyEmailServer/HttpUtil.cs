﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace TestCommonUtils
{
    public class HttpUtil
    {
        public static Response Get(string url, CookieContainer cookies)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            HttpClient client = new HttpClient(handler);
            var res = client.GetAsync(url);
            Response response = new Response()
            {
                Json = res.Result.Content.ReadAsStringAsync().Result,
                Cookies = cookies
            };
            handler.Dispose();
            client.Dispose();
            return response;
        }
    }
}
