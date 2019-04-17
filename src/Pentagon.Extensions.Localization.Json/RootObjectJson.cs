// -----------------------------------------------------------------------
//  <copyright file="CultureManager.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Json.Json
{
    using System.Collections.Generic;
    using IO.Json;
    using Newtonsoft.Json;

    public class RootObjectJson
    {
        [JsonProperty("culture")]
        public string Culture { get; set; }

        [JsonProperty("format")]
        [JsonConverter(typeof(EnumJsonConverter<JsonFormat>))]
        public JsonFormat Format { get; set; } = JsonFormat.KeyValue;

        [JsonProperty("resources")]
        public List<ResourceJson> Resources { get; set; }
    }
}