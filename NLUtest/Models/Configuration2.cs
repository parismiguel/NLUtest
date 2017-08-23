using IBM.WatsonDeveloperCloud.Discovery.v1.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NLUtest.Models
{
    public class Configuration2
    {
        [JsonProperty("configuration_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ConfigurationId { get; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("created", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Created { get; }
        [JsonProperty("updated", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Updated { get; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("conversions", NullValueHandling = NullValueHandling.Ignore)]
        public Conversions Conversions { get; set; }
        [JsonProperty("enrichments", NullValueHandling = NullValueHandling.Ignore)]
        public List<Enrichment2> Enrichments { get; set; }
        [JsonProperty("normalizations", NullValueHandling = NullValueHandling.Ignore)]
        public List<NormalizationOperation> Normalizations { get; set; }
    }
}