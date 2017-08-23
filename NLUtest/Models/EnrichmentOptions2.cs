using IBM.WatsonDeveloperCloud.NaturalLanguageUnderstanding.v1.Model;
using Newtonsoft.Json;

namespace NLUtest.Models
{
    public class EnrichmentOptions2
    {
        [JsonProperty("features", NullValueHandling = NullValueHandling.Ignore)]
        public Features Features { get; set; }
    }
}