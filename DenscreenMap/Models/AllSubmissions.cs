using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DenscreenMap.Models
{
    public partial class AllSubmissions
    {
        [JsonProperty("submissions")]
        public List<Submission> Submissions { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("pages")]
        public long Pages { get; set; }
    }

    public partial class Submission
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("user_agent")]
        public string UserAgent { get; set; }

        [JsonProperty("remote_addr")]
        public string RemoteAddr { get; set; }

        [JsonProperty("payment_status")]
        public string PaymentStatus { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("read")]
        public long Read { get; set; }
    }
}