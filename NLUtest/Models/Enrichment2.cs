using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NLUtest.Models
{
    public class Enrichment2
    {

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("destination_field", NullValueHandling = NullValueHandling.Ignore)]
        public string DestinationField { get; set; }
        [JsonProperty("source_field", NullValueHandling = NullValueHandling.Ignore)]
        public string SourceField { get; set; }
        [JsonProperty("overwrite", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Overwrite { get; set; }
        [JsonProperty("enrichment", NullValueHandling = NullValueHandling.Ignore)]
        public string EnrichmentName { get; set; }
        [JsonProperty("ignore_downstream_errors", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IgnoreDownstreamErrors { get; set; }

        [JsonProperty("options", NullValueHandling = NullValueHandling.Ignore)]
        public EnrichmentOptions2 Options { get; set; }
    }
}
