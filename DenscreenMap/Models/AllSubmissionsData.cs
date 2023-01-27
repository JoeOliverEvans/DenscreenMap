using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DenscreenMap.Models
{
    public partial class Welcome
    {
        [JsonProperty("submissions")]
        public List<SubmissionData> Submissions { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("pages")]
        public long Pages { get; set; }
    }

    public partial class SubmissionData
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

        [JsonProperty("data")]
        public Dictionary<string, DatumData> Data { get; set; }
    }

    public partial class DatumData
    {
        [JsonProperty("field")]
        public long Field { get; set; }

        [JsonProperty("flat_value")]
        public string FlatValue { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }
}