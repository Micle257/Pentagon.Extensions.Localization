namespace Pentagon.Extensions.Localization.Json {
    using Newtonsoft.Json;

    public class ResourceJson
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}