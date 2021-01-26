using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestCommonUtils
{
    public class Email
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("ib")]
        public string EmailAddress { get; set; }

        [JsonProperty("f")]
        public string From { get; set; }

        [JsonProperty("s")]
        public string Subject { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
