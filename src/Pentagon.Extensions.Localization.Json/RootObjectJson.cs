// -----------------------------------------------------------------------
//  <copyright file="CultureManager.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Json.Json
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class RootObjectJson
    {
        [JsonProperty("culture")]
        public string Culture { get; set; }

        [JsonProperty("resources")]
        public List<ResourceJson> Resources { get; set; }
    }
}