using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CrossNews.Core.Model.Api
{
    public class Item : IStory, IComment, IJob, IPoll, IPollOption
    {
        public int Id { get; set; }

        public bool Deleted { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ItemType Type { get; set; }

        public string By { get; set; }

        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Time { get; set; }

        public string Text { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Dead { get; set; }

        public int Parent { get; set; }

        public int Poll { get; set; }

        public List<int> Kids { get; set; }

        public string Url { get; set; }

        public int Score { get; set; }

        public string Title { get; set; }

        public List<int> Parts { get; set; }

        public int Descendants { get; set; }
    }
}
