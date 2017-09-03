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
    /// A response containing the documents and aggregations for the query.
    /// </summary>
    public class QueryResponse
    {
        /// <summary>
        /// Gets or Sets MatchingResults
        /// </summary>
        [JsonProperty("matching_results", NullValueHandling = NullValueHandling.Ignore)]
        public long? MatchingResults { get; set; }
        /// <summary>
        /// Gets or Sets Results
        /// </summary>
        [JsonProperty("results", NullValueHandling = NullValueHandling.Ignore)]
        public List<QueryResult> Results { get; set; }
        /// <summary>
        /// Gets or Sets Aggregations
        /// </summary>
        [JsonProperty("aggregations", NullValueHandling = NullValueHandling.Ignore)]
        public List<QueryAggregation> Aggregations { get; set; }
    }





    public class Extracted_Metadata
    {
        public string publicationdate { get; set; }
        public string sha1 { get; set; }
        public string author { get; set; }
        public string filename { get; set; }
        public string file_type { get; set; }
        public string title { get; set; }
    }

    public class Enriched_Text
    {
        public Entity[] entities { get; set; }
        public Sentiment sentiment { get; set; }
        public Semantic_Roles[] semantic_roles { get; set; }
        public Concept[] concepts { get; set; }
        public Category[] categories { get; set; }
        public object[] relations { get; set; }
        public Keyword2[] keywords { get; set; }
    }

    public class Sentiment
    {
        public Document document { get; set; }
    }

    public class Document
    {
        public float score { get; set; }
        public string label { get; set; }
    }

    public class Entity
    {
        public string type { get; set; }
        public string text { get; set; }
        public Disambiguation disambiguation { get; set; }
        public int count { get; set; }
    }

    public class Disambiguation
    {
        public string[] subtype { get; set; }
    }

    public class Semantic_Roles
    {
        public Subject subject { get; set; }
        public string sentence { get; set; }
        public Action action { get; set; }
        public Object _object { get; set; }
    }

    public class Subject
    {
        public string text { get; set; }
        public Keyword[] keywords { get; set; }
        public Entity1[] entities { get; set; }
    }

    public class Keyword
    {
        public string text { get; set; }
    }

    public class Entity1
    {
        public string type { get; set; }
        public string text { get; set; }
        public Disambiguation1 disambiguation { get; set; }
    }

    public class Disambiguation1
    {
        public string[] subtype { get; set; }
        public string name { get; set; }
        public string dbpedia_resource { get; set; }
    }

    public class Action
    {
        public string text { get; set; }
        public Verb verb { get; set; }
    }

    public class Verb
    {
        public bool negated { get; set; }
    }

    public class Object
    {
        public string text { get; set; }
        public Entity2[] entities { get; set; }
        public Keyword1[] keywords { get; set; }
    }

    public class Entity2
    {
        public string type { get; set; }
        public string text { get; set; }
        public Disambiguation2 disambiguation { get; set; }
    }

    public class Disambiguation2
    {
        public string[] subtype { get; set; }
        public string name { get; set; }
        public string dbpedia_resource { get; set; }
    }

    public class Keyword1
    {
        public string text { get; set; }
    }

    public class Concept
    {
        public string text { get; set; }
        public float relevance { get; set; }
        public string dbpedia_resource { get; set; }
    }

    public class Category
    {
        public float score { get; set; }
        public string label { get; set; }
    }

    public class Keyword2
    {
        public string text { get; set; }
        public float relevance { get; set; }
    }




}
