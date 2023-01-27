using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DenscreenMap.Models
{
    public partial class SpecificSubmission 
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

        [JsonProperty("form")]
        public long Form { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("data")]
        public List<Datum> Data { get; set; }
    }

    public partial class Datum
    {
        [JsonProperty("field")]
        public long Field { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}