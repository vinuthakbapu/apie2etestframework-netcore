using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestCommonUtils
{
    public class InboxEmail
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("ib")]
        public string EmailAddress { get; set; }

        [JsonProperty("f")]
        public string From { get; set; }

        [JsonProperty("s")]
        public string Subject { get; set; }

        [JsonProperty("fe")]
        public string Sender { get; set; }

        [JsonProperty("rf")]
        public string EmailTimestamp { get; set; }
    }
}
