/**
* Copyright 2017 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

using System.Collections.Generic;
using Newtonsoft.Json;

namespace IBM.WatsonDeveloperCloud.Discovery.v1.Model
{
    /// <summary>
    /// QueryResult.
    /// </summary>
    public class QueryResult
    {
        /// <summary>
        /// The unique identifier of the document.
        /// </summary>
        /// <value>The unique identifier of the document.</value>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        /// <summary>
        /// The confidence score of the result's analysis. Scores range from 0 to 1, with a higher score indicating greater confidence.
        /// </summary>
        /// <value>The confidence score of the result's analysis. Scores range from 0 to 1, with a higher score indicating greater confidence.</value>
        [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
        public double? Score { get; set; }
        /// <summary>
        /// Metadata of the document.
        /// </summary>
        /// <value>Metadata of the document.</value>
        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        public object Metadata { get; set; }

        [JsonProperty("extracted_metadata", NullValueHandling = NullValueHandling.Ignore)]
        public object Extracted_Metadata { get; set; }

        [JsonProperty("html", NullValueHandling = NullValueHandling.Ignore)]
        public string html { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string text { get; set; }

        [JsonProperty("enriched_text", NullValueHandling = NullValueHandling.Ignore)]
        public Enriched_Text_Result enriched_text { get; set; }
    }


    public class Enriched_Text_Result
    {
        public EntityResult[] entities { get; set; }
        public SentimentResult sentiment { get; set; }
        public Semantic_RolesResult[] semantic_roles { get; set; }
        public ConceptResult[] concepts { get; set; }
        public CategoryResult[] categories { get; set; }
        public RelationResult[] relations { get; set; }
        public KeywordResult[] keywords { get; set; }
    }

    public class SentimentResult
    {
        public DocumentResult document { get; set; }
    }

    public class DocumentResult
    {
        public float score { get; set; }
        public string label { get; set; }
    }

    public class EntityResult
    {
        public string type { get; set; }
        public string text { get; set; }
        public Disambiguation0Result disambiguation { get; set; }
        public int count { get; set; }
    }

    public class Disambiguation0Result
    {
        public string[] subtype { get; set; }
    }

    public class Semantic_RolesResult
    {
        public string sentence { get; set; }
        public ObjectResult _object { get; set; }
        public ActionResult action { get; set; }
    }

    public class ObjectResult
    {
        public string text { get; set; }
    }

    public class ActionResult
    {
        public string text { get; set; }
    }

    public class ConceptResult
    {
        public string text { get; set; }
        public float relevance { get; set; }
        public string dbpedia_resource { get; set; }
    }

    public class CategoryResult
    {
        public float score { get; set; }
        public string label { get; set; }
    }

    public class RelationResult
    {
        public string type { get; set; }
        public string sentence { get; set; }
        public float score { get; set; }
        public ArgumentResult[] arguments { get; set; }
    }

    public class ArgumentResult
    {
        public string text { get; set; }
        public Entity1Result[] entities { get; set; }
    }

    public class Entity1Result
    {
        public string type { get; set; }
        public string text { get; set; }
        public Disambiguation1Result disambiguation { get; set; }
    }

    public class Disambiguation1Result
    {
        public string[] subtype { get; set; }
    }

    public class KeywordResult
    {
        public string text { get; set; }
        public float relevance { get; set; }
    }



}
