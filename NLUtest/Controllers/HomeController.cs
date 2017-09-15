using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLUtest.Models;
using IBM.WatsonDeveloperCloud.NaturalLanguageUnderstanding.v1;
using IBM.WatsonDeveloperCloud.NaturalLanguageUnderstanding.v1.Model;
using IBM.WatsonDeveloperCloud.Discovery.v1.Model;
using System.Threading;
using IBM.WatsonDeveloperCloud.Discovery.v1;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using IBM.WatsonDeveloperCloud.VisualRecognition.v3;
using System.Net;
using System.Text.RegularExpressions;

namespace NLUtest.Controllers
{

    public class HomeController : Controller
    {
        #region NLU parameters
        private NaturalLanguageUnderstandingService _naturalLanguageUnderstandingService;

        string userNLU = "41e3eac1-88e0-4ed9-aca2-b7c2d74bdcc1";
        string pswNLU = "442L2dJkoqAO";

        string modelNLU = "";
        #endregion

        #region Visual Recognition parameters
        // create a Tone Analyzer Service instance
        private VisualRecognitionService _visualRecognition = new VisualRecognitionService();
        private static string _apikey = "1035407f25f1e143bac6598920ddfae8786bec7e";

        #endregion


        #region Discovery Parameters
        public string _username = "20312444-1393-49f5-9bce-ab73bfc08c19";
        public string _password = "Z1DxzUeHnTP0";
        public string _endpoint;
        public DiscoveryService _discovery;

        private static string _createdEnvironmentId = "b1612bd9-6f9f-485f-84f4-c9d4740f509b";
        //private static string _createdConfigurationId = "31d60f6f-dc38-48bf-be56-d62ce5571594";
        //private static string _createdCollectionId = "9fe7fde3-3ffa-4773-856f-f88e2a8695e6";
        private static string _createdDocumentId;

        //private string _createdEnvironmentName = "dotnet-test-environment";
        //private string _createdEnvironmentDescription = "Ambiente creado desde C# Core PMP";
        //private int _createdEnvironmentSize = 1;
        //private string _updatedEnvironmentName = "dotnet-test-environment-updated";
        //private string _updatedEnvironmentDescription = "Ambiente creado desde C# Core PMP - Actualizado";
        //private string _createdConfigurationName = "configName";
        //private string _updatedConfigurationName = "configName-updated";
        //private string _createdConfigurationDescription = "configDescription";

        private string _filepathToIngest = @"DiscoveryTestData\watson_beats_jeopardy.html";
        private string _metadata = "{\"Creator\": \"Paris Pantigoso\",\"Subject\": \"Discovery example\"}";

        //private string _createdCollectionName = "createdCollectionName";
        //private string _createdCollectionDescription = "createdCollectionDescription";
        //private string _updatedCollectionName = "updatedCollectionName";
        //private CreateCollectionRequest.LanguageEnum _createdCollectionLanguage = CreateCollectionRequest.LanguageEnum.ES;

        //private string _naturalLanguageQuery = "Who beat Ken Jennings in Jeopardy!";

        AutoResetEvent autoEvent = new AutoResetEvent(false);
        #endregion

        //private string _nluText = "Analyze various features of text content at scale. Provide text, raw HTML, or a public URL, and IBM Watson Natural Language Understanding will give you results for the features you request. The service cleans HTML content before analysis by default, so the results can ignore most advertisements and other unwanted content.";

        private string _nluText;
        private string _nluModel;

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult NLU()
        {

            AnalysisResults model = new AnalysisResults();
            ViewData["modelID"] = "10:7beae556-4bc7-4873-9506-039173e76037";

            try
            {
                //_nluText = Get_HTML("https://www.tripadvisor.com.pe/ShowUserReviews-g946508-d2308972-r263835732-Restaurant_El_Refugio_de_Mamaine-Chincha_Alta_Ica_Region.html");

                //model.AnalyzedText = "Luego de 10 años, volví a este restaurante. Sin duda alguna, su plato fuerte es la Sopa Seca con Carapulcra, es la mejor que he probado. La atención es A1. Eso sí, en fechas festivas es mejor ir desde temprano ya que el lugar, a pesar de ser grande, se llena rápidamente.";
                model.AnalyzedText = System.IO.File.ReadAllText(@"..\NLUtest\wwwroot\Donde puedo comer el mejor lomo saltado.txt");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                model.AnalyzedText = "Luego de 10 años, volví a este restaurante. Sin duda alguna, su plato fuerte es la Sopa Seca con Carapulcra, es la mejor que he probado. La atención es A1. Eso sí, en fechas festivas es mejor ir desde temprano ya que el lugar, a pesar de ser grande, se llena rápidamente.";

                return View(model);
            }

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NLU(string AnalyzedText, string queryModelID)
        {
            _nluText = AnalyzedText;
            _nluModel = queryModelID;

            ViewData["modelID"] = _nluModel;

            AnalysisResults model = NaturalLanguageUnderstandingExample(userNLU, pswNLU);

            return View(model);
        }

        public IActionResult Discovery()
        {
            _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);

            //ViewData["Enviroments"] = JsonConvert.SerializeObject(GetEnvironments(), Formatting.Indented);
            //ViewData["Configurations"] = GetConfigurations();
            //ViewData["Collections"] = GetCollections();
            ViewData["Documents"] = GetDocuments();
            ViewData["queryDefault"] = "¿Donde puedo comer el mejor lomo saltado?";

            ViewData["modelID"] = "04529d40-bd37-4de1-af66-ed08fadd0e5c";

            return View();
        }

        #region Environments

        [HttpPost]
        public IActionResult GetEnvironments()
        {
            _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);

            var result = _discovery.ListEnvironments();

            if (result != null)
            {
                //var model = JsonConvert.SerializeObject(result, Formatting.Indented);

                return Json(result);

            }
            else
            {
                return Json("Resultado nulo");
            }
        }


        #endregion

        #region Configurations

        [HttpPost]
        public IActionResult GetConfigurations()
        {
            _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);

            var result = _discovery.ListConfigurations(_createdEnvironmentId);

            if (result != null)
            {
                //var model = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Json(result);
            }
            else
            {
                return Json("Resultado nulo");
            }
        }

        [HttpPost]
        public IActionResult GetConfiguration(string configid)
        {

            _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);

            var result = _discovery.GetConfiguration(_createdEnvironmentId, configid);

            if (result != null)
            {
                //var model = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Json(result);
            }
            else
            {
                return Json("Resultado nulo");
            }

        }

        [HttpPost]
        public IActionResult CreateConfiguration(string configname, string modelid)
        {
            _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);


            Conversions _conversions = new Conversions()
            {
                Pdf = new PdfSettings()
                {
                    Heading = new PdfHeadingDetection()
                    {
                        Fonts = new List<FontSetting>()
                        {
                           new FontSetting()
                           {
                               Level = (float)1.0,
                               MinSize = (float)24.0,
                               MaxSize = (float)80.0
                           },
                           new FontSetting()
                           {
                               Level = (float)2.0,
                               MinSize = (float)18.0,
                               MaxSize = (float)24.0,
                               Bold = false,
                               Italic = false
                           },
                           new FontSetting()
                           {
                               Level = (float)2.0,
                               MinSize = (float)18.0,
                               MaxSize = (float)24.0,
                               Bold = true
                           },
                           new FontSetting()
                           {
                               Level = (float)3.0,
                               MinSize = (float)13.0,
                               MaxSize = (float)18.0,
                               Bold = false,
                               Italic = false
                           },
                           new FontSetting()
                           {
                               Level = (float)3.0,
                               MinSize = (float)13.0,
                               MaxSize = (float)18.0,
                               Bold = true
                           },
                           new FontSetting()
                           {
                               Level = (float)4.0,
                               MinSize = (float)11.0,
                               MaxSize = (float)13.0,
                               Bold = false,
                               Italic = false
                           }
                        }
                    }
                },
                Word = new WordSettings()
                {
                    Heading = new WordHeadingDetection()
                    {
                        Fonts = new List<FontSetting>()
                        {
                           new FontSetting()
                           {
                               Level = (float)1.0,
                               MinSize = (float)24.0,
                               Bold = false,
                               Italic = false
                           },
                           new FontSetting()
                           {
                               Level = (float)2.0,
                               MinSize = (float)18.0,
                               MaxSize = (float)23.0,
                               Bold = true,
                               Italic = false
                           },
                            new FontSetting()
                           {
                               Level = (float)3.0,
                               MinSize = (float)14.0,
                               MaxSize = (float)17.0,
                               Bold = false,
                               Italic = false
                           },
                             new FontSetting()
                           {
                               Level = (float)4.0,
                               MinSize = (float)13.0,
                               MaxSize = (float)13.0,
                               Bold = true,
                               Italic = false
                           }
                        },
                        Styles = new List<WordStyle>()
                        {
                            new WordStyle()
                            {
                                Level = (float)1.0,
                                Names = new List<string>()
                                {
                                    "pullout heading","pulloutheading","header"
                                }
                            },
                            new WordStyle()
                            {
                                Level = (float)2.0,
                                Names = new List<string>()
                                {
                                    "subtitle"
                                }
                            }
                        }
                    }
                },
                Html = new HtmlSettings()
                {
                    ExcludeTagsCompletely = new List<string>()
                    {
                        "script","sup"
                    },
                    ExcludeTagsKeepContent = new List<string>()
                    {
                        "font","em","span"
                    },
                    KeepContent = new XPathPatterns()
                    {
                        Xpaths = new List<string>()
                    },
                    ExcludeContent = new XPathPatterns()
                    {
                        Xpaths = new List<string>()
                    },
                    ExcludeTagAttributes = new List<string>()
                    {
                        "EVENT_ACTIONS"
                    }
                },
                JsonNormalizations = new List<NormalizationOperation>()
            };

            Configuration2 configuration = new Configuration2()
            {
                Name = configname,
                Description = "Creado desde API",
                Conversions = _conversions,
                Enrichments = new List<Enrichment2>()
                {
                    new Enrichment2()
                    {
                        DestinationField = "enriched_text",
                        SourceField = "text",
                        EnrichmentName = "natural_language_understanding",
                        Options = new EnrichmentOptions2()
                    }
                }
            };



            Features _features = new Features()
            {
                Entities = new EntitiesOptions()
                {
                    Sentiment = true,
                    Emotion = true,
                    Limit = 20,
                    Model = modelid
                },
                Sentiment = new SentimentOptions()
                {
                    Document = true
                },
                Categories = new CategoriesOptions(),
                Concepts = new ConceptsOptions()
                {
                    Limit = 8
                },

                Relations = new RelationsOptions()
                {
                    Model = modelid
                },

                Emotion = new EmotionOptions()
                {
                    Document = true
                },
                Keywords = new KeywordsOptions()
                {
                    Limit = 20,
                    Sentiment = true,
                    Emotion = true
                },
                SemanticRoles = new SemanticRolesOptions()
                {
                    Limit = 8,
                    Entities = true,
                    Keywords = true
                }
            };


            if (!string.IsNullOrEmpty(modelid))
            {
                configuration.Enrichments[0].Options.Features = _features;
            }
            else
            {
                configuration.Enrichments[0].Options = new EnrichmentOptions2();
            }


            var result = _discovery.CreateConfiguration(_createdEnvironmentId, configuration);

            if (result != null)
            {
                var model = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Json(model);

            }
            else
            {
                return Json("Resultado nulo");
            }
        }

        [HttpPost]
        public IActionResult UpdateConfiguration(string configid, string modelid)
        {
            _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);

            Conversions _conversions = new Conversions()
            {
                Pdf = new PdfSettings()
                {
                    Heading = new PdfHeadingDetection()
                    {
                        Fonts = new List<FontSetting>()
                        {
                           new FontSetting()
                           {
                               Level = (float)1.0,
                               MinSize = (float)24.0,
                               MaxSize = (float)80.0
                           },
                           new FontSetting()
                           {
                               Level = (float)2.0,
                               MinSize = (float)18.0,
                               MaxSize = (float)24.0,
                               Bold = false,
                               Italic = false
                           },
                           new FontSetting()
                           {
                               Level = (float)2.0,
                               MinSize = (float)18.0,
                               MaxSize = (float)24.0,
                               Bold = true
                           },
                           new FontSetting()
                           {
                               Level = (float)3.0,
                               MinSize = (float)13.0,
                               MaxSize = (float)18.0,
                               Bold = false,
                               Italic = false
                           },
                           new FontSetting()
                           {
                               Level = (float)3.0,
                               MinSize = (float)13.0,
                               MaxSize = (float)18.0,
                               Bold = true
                           },
                           new FontSetting()
                           {
                               Level = (float)4.0,
                               MinSize = (float)11.0,
                               MaxSize = (float)13.0,
                               Bold = false,
                               Italic = false
                           }
                        }
                    }
                },
                Word = new WordSettings()
                {
                    Heading = new WordHeadingDetection()
                    {
                        Fonts = new List<FontSetting>()
                        {
                           new FontSetting()
                           {
                               Level = (float)1.0,
                               MinSize = (float)24.0,
                               Bold = false,
                               Italic = false
                           },
                           new FontSetting()
                           {
                               Level = (float)2.0,
                               MinSize = (float)18.0,
                               MaxSize = (float)23.0,
                               Bold = true,
                               Italic = false
                           },
                            new FontSetting()
                           {
                               Level = (float)3.0,
                               MinSize = (float)14.0,
                               MaxSize = (float)17.0,
                               Bold = false,
                               Italic = false
                           },
                             new FontSetting()
                           {
                               Level = (float)4.0,
                               MinSize = (float)13.0,
                               MaxSize = (float)13.0,
                               Bold = true,
                               Italic = false
                           }
                        },
                        Styles = new List<WordStyle>()
                        {
                            new WordStyle()
                            {
                                Level = (float)1.0,
                                Names = new List<string>()
                                {
                                    "pullout heading","pulloutheading","header"
                                }
                            },
                            new WordStyle()
                            {
                                Level = (float)2.0,
                                Names = new List<string>()
                                {
                                    "subtitle"
                                }
                            }
                        }
                    }
                },
                Html = new HtmlSettings()
                {
                    ExcludeTagsCompletely = new List<string>()
                    {
                        "script","sup"
                    },
                    ExcludeTagsKeepContent = new List<string>()
                    {
                        "font","em","span"
                    },
                    KeepContent = new XPathPatterns()
                    {
                        Xpaths = new List<string>()
                    },
                    ExcludeContent = new XPathPatterns()
                    {
                        Xpaths = new List<string>()
                    },
                    ExcludeTagAttributes = new List<string>()
                    {
                        "EVENT_ACTIONS"
                    }
                },
                JsonNormalizations = new List<NormalizationOperation>()
            };

            Enrichment2 _enrichment = new Enrichment2()
            {
                DestinationField = "enriched_text",
                SourceField = "text",
                EnrichmentName = "natural_language_understanding",
                Options = new EnrichmentOptions2()
            };

            Features _features = new Features()
            {
                Entities = new EntitiesOptions()
                {
                    Sentiment = true,
                    Emotion = true,
                    Limit = 50,
                    Model = modelid
                },
                Sentiment = new SentimentOptions()
                {
                    Document = true
                },
                Categories = new CategoriesOptions(),
                Concepts = new ConceptsOptions()
                {
                    Limit = 10
                },

                Relations = new RelationsOptions()
                {
                    Model = modelid
                },

                Emotion = new EmotionOptions()
                {
                    Document = true
                },
                Keywords = new KeywordsOptions()
                {
                    Limit = 30,
                    Sentiment = true,
                    Emotion = true
                },
                SemanticRoles = new SemanticRolesOptions()
                {
                    Limit = 10,
                    Entities = true,
                    Keywords = true
                }
            };


            if (!string.IsNullOrEmpty(modelid))
            {
                _enrichment.Options.Features = _features;
            }
            else
            {
                _enrichment.Options = new EnrichmentOptions2();
            }


            Configuration2 config = _discovery.GetConfiguration(_createdEnvironmentId, configid);

            config.Enrichments[0] = _enrichment;
            config.Conversions = _conversions;


            var result = _discovery.UpdateConfiguration(_createdEnvironmentId, configid, config);

            if (result != null)
            {
                var model = JsonConvert.SerializeObject(result.Enrichments[0], Formatting.Indented);
                return Json(model);
            }
            else
            {
                return Json("Resultado nulo");
            }
        }

        [HttpPost]
        public IActionResult DeleteConfiguration(string configid)
        {
            _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);

            var result = _discovery.DeleteConfiguration(_createdEnvironmentId, configid);

            if (result != null)
            {
                var model = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Json(model);
            }
            else
            {
                return Json("Resultado nulo");
            }
        }


        #endregion

        #region Collections

        [HttpPost]
        public IActionResult GetCollections()
        {
            _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);

            var result = _discovery.ListCollections(_createdEnvironmentId);

            if (result != null)
            {
                //var model = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Json(result);
            }
            else
            {
                return Json("Resultado nulo");
            }
        }

        [HttpPost]
        public IActionResult GetCollection(string collectionid)
        {
            _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);

            var result = _discovery.GetCollection(_createdEnvironmentId, collectionid);

            if (result != null)
            {
                //var model = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Json(result);
            }
            else
            {
                return Json("Resultado nulo");
            }
        }

        [HttpPost]
        public IActionResult CreateCollection(string name, string configid)
        {
            _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);

            CreateCollectionRequest createCollectionRequest = new CreateCollectionRequest()
            {
                Language = CreateCollectionRequest.LanguageEnum.ES,
                Name = name,
                ConfigurationId = configid
            };

            var result = _discovery.CreateCollection(_createdEnvironmentId, createCollectionRequest);

            if (result != null)
            {
                var model = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Json(model);
            }
            else
            {
                return Json("Resultado nulo");
            }
        }

        [HttpPost]
        public IActionResult DeleteCollection(string collectionid)
        {

            if (string.IsNullOrEmpty(_createdEnvironmentId))
                throw new ArgumentNullException("_createdEnvironmentId es nulo");

            _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);

            var result = _discovery.DeleteCollection(_createdEnvironmentId, collectionid);

            if (result != null)
            {
                var model = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Json(model);
            }
            else
            {
                return Json("Resultado nulo");
            }
        }


        #endregion

        #region Documents

        private string GetDocuments()
        {
            var collectionid = "9fe7fde3-3ffa-4773-856f-f88e2a8695e6";
            var query = "return=extracted_metadata&version=2017-08-01";
            //var query = "return=extracted_metadata";

            var result = _discovery.Query(_createdEnvironmentId, collectionid, null, query);

            if (result != null)
            {
                var model = JsonConvert.SerializeObject(result, Formatting.Indented);
                return model;
            }
            else
            {
                return "Resultado nulo";
            }
        }

        [HttpPost]
        public IActionResult AddDocument(string collectionid, string configid)
        {
            //_createdEnvironmentId = "b1612bd9-6f9f-485f-84f4-c9d4740f509b"; //byod
            //collectionid = "9fe7fde3-3ffa-4773-856f-f88e2a8695e6"; //JamaPeruCollection
            //configid = "9726e2b6-5671-41c0-bd4d-2a9512ce02ee"; //Config20170801

            //_filepathToIngest = @"DiscoveryTestData\watson_beats_jeopardy.html";
            //_metadata = "{\"Creator\": \"Paris Pantigoso\",\"Subject\": \"Discovery example\"}";

            _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);


            using (FileStream fs = System.IO.File.OpenRead(_filepathToIngest))
            {
                var result = _discovery.AddDocument(_createdEnvironmentId, collectionid, configid, fs as Stream, _metadata);

                if (result != null)
                {
                    var model = JsonConvert.SerializeObject(result, Formatting.Indented);
                    _createdDocumentId = result.DocumentId;

                    return Json(result.DocumentId);
                }
                else
                {
                    return Json("Resultado nulo");
                }
            }
        }

        [HttpPost]
        public IActionResult AddDocumentList(List<string> list)
        {
            //_createdEnvironmentId = "b1612bd9-6f9f-485f-84f4-c9d4740f509b"; //byod

            var collectionid = "9fe7fde3-3ffa-4773-856f-f88e2a8695e6"; //JamaPeruCollection
            var configid = "9726e2b6-5671-41c0-bd4d-2a9512ce02ee"; //Config20170801

            //_filepathToIngest = @"DiscoveryTestData\watson_beats_jeopardy.html";
            //_metadata = "{\"file_type\": \"text/html\",\"creator\": \"Paris Pantigoso\",\"subject\": \"Discovery example\"}";

            _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);

            var listResult = new List<DocumentAccepted>();

            //int counter = 0;

            foreach (var item in list)
            {
                //if (counter == 1)
                //{
                //    return Json(listResult);
                //}

                using (WebClient client = new WebClient())
                {

                    byte[] byteArray = client.DownloadData(item);

                    string fileType = client.ResponseHeaders[HttpResponseHeader.ContentType];

                    string source = client.DownloadString(item);
                    string _title = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;

                    using (MemoryStream _stream = new MemoryStream(byteArray))
                    {
                        DocumentAccepted result = _discovery.AddDocument(_createdEnvironmentId, collectionid, configid, _stream as Stream, _metadata, null, _title);

                        if (result != null)
                        {
                            listResult.Add(result);
                        }
                    }

                }


                //counter++;

            }

            return Json(listResult);

        }



        [HttpPost]
        public IActionResult DetailsDocument(string collectionid, string documentid)
        {
            _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);

            var result = _discovery.GetDocumentStatus(_createdEnvironmentId, collectionid, documentid);

            if (result != null)
            {
                //var model = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Json(result);
            }
            else
            {
                return Json("Resultado nulo");
            }
        }

        [HttpPost]
        public IActionResult DeleteDocument(string collectionid, string documentid)
        {
            _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);

            var result = _discovery.DeleteDocument(_createdEnvironmentId, collectionid, documentid);

            if (result != null)
            {
                var model = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Json(model);
            }
            else
            {
                return Json("Resultado nulo");
            }
        }

        #endregion


        #region Query

        [HttpPost]
        public IActionResult QueryRequest(string environmentid, string collectionid, string queryrequest)
        {
            QueryResponse model = new QueryResponse();

            try
            {
                _discovery = new DiscoveryService(_username, _password, DiscoveryService.DISCOVERY_VERSION_DATE_2017_08_01);

                var result = _discovery.Query(_createdEnvironmentId, collectionid, null, null, queryrequest);

                if (result != null)
                {
                    //model = JsonConvert.SerializeObject(result, Formatting.Indented);
                    model = result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);

                return Json(ex.Message);
            }

            return Json(model);
        }
        #endregion


        public IActionResult VisualR()
        {
            ViewData["Message"] = "Visual Recognition";

            //https://github.com/watson-developer-cloud/dotnet-standard-sdk/tree/development/src/IBM.WatsonDeveloperCloud.VisualRecognition.v3

            // set the credentials
            _visualRecognition.SetCredential(_apikey);



            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Constructor
        public AnalysisResults NaturalLanguageUnderstandingExample(string username, string password)
        {
            _naturalLanguageUnderstandingService = new NaturalLanguageUnderstandingService(username, password, NaturalLanguageUnderstandingService.NATURAL_LANGUAGE_UNDERSTANDING_VERSION_DATE_2017_02_27);

            AnalysisResults model = new AnalysisResults();

            model = Analyze(_nluModel);
            return model;

        }
        #endregion

        #region Analyze
        public AnalysisResults Analyze(string queryModelID)
        {
            List<string> model = new List<string>();

            List<string> _emotions = new List<string>()
            {
                "anger",
                "disgust",
                "fear",
                "joy",
                "sadness"
            };

            List<string> _targets = new List<string>()
            {
                "comida",
                "restaurante",
                "plato"
            };

            Parameters parameters = new Parameters()
            {
                Clean = true,
                FallbackToRaw = true,
                ReturnAnalyzedText = true,
                Features = new Features()
                {
                    Relations = new RelationsOptions(),
                    Sentiment = new SentimentOptions()
                    {
                        Document = true
                        //Targets = _targets
                    },
                    Emotion = new EmotionOptions()
                    {
                        Document = true
                        //Targets = _emotions
                    },
                    Keywords = new KeywordsOptions()
                    {
                        Limit = 50,
                        Sentiment = true,
                        Emotion = true
                    },
                    Entities = new EntitiesOptions()
                    {
                        Limit = 50,
                        Emotion = true,
                        Sentiment = true
                    },
                    Categories = new CategoriesOptions(),
                    Concepts = new ConceptsOptions()
                    {
                        Limit = 8
                    },
                    SemanticRoles = new SemanticRolesOptions()
                    {
                        Limit = 50,
                        Entities = true,
                        Keywords = true
                    }
                }
            };

            if (!string.IsNullOrEmpty(queryModelID))
            {
                parameters.Features.Relations.Model = queryModelID;
                parameters.Features.Entities.Model = queryModelID;
            }

            if (_nluText.StartsWith("http"))
            {
                parameters.Url = _nluText;
                parameters.Features.Metadata = new MetadataOptions();
            }
            else
            {
                parameters.Text = _nluText;
            }

            //parameters.Language = "en";

            Console.WriteLine(string.Format("\nAnalizando()..."));

            AnalysisResults result = _naturalLanguageUnderstandingService.Analyze(parameters);

            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.Language))

                    model.Add(string.Format("Idioma: {0}", result.Language));

                if (!string.IsNullOrEmpty(result.AnalyzedText))

                    model.Add(string.Format("Texto analizado: {0}", result.AnalyzedText));

                if (!string.IsNullOrEmpty(result.RetrievedUrl))

                    model.Add(string.Format("URL recuperado: {0}", result.RetrievedUrl));

                if (result.Usage != null)
                {
                    if (result.Usage.Features != null)
                        model.Add(string.Format("Caracerísticas de uso: {0}", result.Usage.Features));
                }

                if (result.Concepts != null && result.Concepts.Count > 0)
                {
                    foreach (ConceptsResult conceptResult in result.Concepts)
                    {
                        model.Add(string.Format("Resultados de Concepto: {0}, Relevancia {1}, Fuente {2}", conceptResult.Text, conceptResult.Relevance, conceptResult.DbpediaResource));
                    }
                }

                if (result.Entities != null && result.Entities.Count > 0)
                {
                    foreach (EntitiesResult entityResult in result.Entities)
                    {
                        model.Add(string.Format("Tipo de Entidad: {0} | Relevancia: {1} | Total: {2} | Texto: {3}", entityResult.Type, entityResult.Relevance, entityResult.Count, entityResult.Text));

                        if (entityResult.Emotion != null)
                        {
                            EmotionScores emotionScores = entityResult.Emotion;
                            model.Add(string.Format("Enojo: {0} | Disgusto: {1} | Miedo: {2} | Alegría: {3} | Tristeza: {4}", emotionScores.Anger, emotionScores.Disgust, emotionScores.Fear, emotionScores.Joy, emotionScores.Sadness));
                        }

                        if (entityResult.Sentiment != null)
                        {
                            if (entityResult.Sentiment.Score != null)
                                model.Add("Puntaje Sentimiento: " + entityResult.Sentiment.Score);
                        }

                        if (entityResult.Disambiguation != null)
                        {
                            DisambiguationResult disambiguationResult = entityResult.Disambiguation;
                            model.Add(string.Format("Desambiguación de nombre: {0} | dbpediaResource: {1}", disambiguationResult.Name, disambiguationResult.DbpediaResource));

                            foreach (string type in disambiguationResult.Subtype)
                            {
                                model.Add("Sub tipo: " + type);
                            }
                        }
                    }
                }

                if (result.Keywords != null && result.Keywords.Count > 0)
                {
                    foreach (KeywordsResult keywordResult in result.Keywords)
                    {
                        model.Add(string.Format("Relevancia de palabras clave: {0}, Texto: {1}", keywordResult.Relevance, keywordResult.Text));

                        if (keywordResult.Emotion != null)
                        {
                            EmotionScores emotionScores = keywordResult.Emotion;
                            model.Add(string.Format("Enojo: {0} | Disgusto: {1} | Miedo: {2} | Alegría: {3} | Tristeza: {4}", emotionScores.Anger, emotionScores.Disgust, emotionScores.Fear, emotionScores.Joy, emotionScores.Sadness));
                        }

                        if (keywordResult.Sentiment != null)
                        {
                            model.Add("Puntaje Sentimiento: " + keywordResult.Sentiment.Score);
                        }
                    }
                }

                if (result.Categories != null && result.Categories.Count > 0)
                {
                    foreach (CategoriesResult categoryResult in result.Categories)
                    {
                        model.Add(string.Format("Categoría: {0} | Puntaje: {1}", categoryResult.Label, categoryResult.Score));
                    }
                }

                if (result.Emotion != null)
                {
                    if (result.Emotion.Document != null)
                    {
                        if (result.Emotion.Document.Emotion != null)
                        {
                            EmotionScores emotionScores = result.Emotion.Document.Emotion;
                            model.Add(string.Format("Enojo: {0} | Disgusto: {1} | Miedo: {2} | Alegría: {3} | Tristeza: {4}", emotionScores.Anger, emotionScores.Disgust, emotionScores.Fear, emotionScores.Joy, emotionScores.Sadness));
                        }
                    }

                    if (result.Emotion.Targets != null && result.Emotion.Targets.Count > 0)
                    {
                        foreach (TargetedEmotionResults targetedEmotionResult in result.Emotion.Targets)
                        {
                            model.Add(string.Format("Resultado Emoción: {0}", targetedEmotionResult.Text));

                            if (targetedEmotionResult.Emotion != null)
                            {
                                EmotionScores emotionScores = targetedEmotionResult.Emotion;
                                model.Add(string.Format("Enojo: {0} | Disgusto: {1} | Miedo: {2} | Alegría: {3} | Tristeza: {4}", emotionScores.Anger, emotionScores.Disgust, emotionScores.Fear, emotionScores.Joy, emotionScores.Sadness));
                            }
                        }

                    }
                }

                if (result.Metadata != null)
                {
                    MetadataResult metadata = result.Metadata;

                    if (metadata.Authors != null && metadata.Authors.Count > 0)
                    {
                        foreach (Author author in metadata.Authors)
                        {
                            model.Add("Autor: " + author.Name);
                        }
                    }
                }

                if (result.Relations != null && result.Relations.Count > 0)
                {
                    foreach (RelationsResult relationResult in result.Relations)
                    {
                        model.Add(string.Format("Puntaje Relación: {0} | Oración: {1} | Tipo: {2}", relationResult.Score, relationResult.Sentence, relationResult.Type));

                        if (relationResult.Arguments != null && relationResult.Arguments.Count > 0)
                        {
                            foreach (RelationArgument arg in relationResult.Arguments)
                            {
                                model.Add("Texto: " + arg.Text);

                                if (arg.Entities != null && arg.Entities.Count > 0)
                                {
                                    foreach (RelationEntity entity in arg.Entities)
                                    {
                                        model.Add(string.Format("Relación de Entidad: {0} | Tipo: {1}", entity.Text, entity.Type));
                                    }
                                }
                            }
                        }
                    }
                }

                if (result.SemanticRoles != null && result.SemanticRoles.Count > 0)
                {
                    foreach (SemanticRolesResult semanticRoleResult in result.SemanticRoles)
                    {
                        model.Add(string.Format("Rol semántica: {0}", semanticRoleResult.Sentence));

                        if (semanticRoleResult.Subject != null)
                        {
                            model.Add(string.Format("Sujeto semántica: {0}", semanticRoleResult.Subject.Text));

                            if (semanticRoleResult.Subject.Entities != null && semanticRoleResult.Subject.Entities.Count > 0)
                            {
                                foreach (SemanticRolesEntity entity in semanticRoleResult.Subject.Entities)
                                {
                                    model.Add(string.Format("Tipo Entidad: {0} | text: {1}", entity.Type, entity.Text));
                                }
                            }

                            if (semanticRoleResult.Subject.Keywords != null && semanticRoleResult.Subject.Keywords.Count > 0)
                            {
                                foreach (SemanticRolesKeyword keyword in semanticRoleResult.Subject.Keywords)
                                {
                                    model.Add(string.Format("Palabra clave: {0}", keyword.Text));
                                }
                            }
                        }

                        if (semanticRoleResult.Action != null)
                        {
                            model.Add(string.Format("Acción: {0} | Normalizado: {1}", semanticRoleResult.Action.Text, semanticRoleResult.Action.Normalized));

                            if (semanticRoleResult.Action.Verb != null)
                            {
                                model.Add(string.Format("Verbo: {0} | Frase: {1}", semanticRoleResult.Action.Verb.Text, semanticRoleResult.Action.Verb.Tense));
                            }
                        }

                        if (semanticRoleResult._Object != null)
                        {
                            model.Add(string.Format("Objeto: {0}", semanticRoleResult._Object.Text));

                            if (semanticRoleResult._Object.Keywords != null && semanticRoleResult._Object.Keywords.Count > 0)
                            {
                                foreach (SemanticRolesKeyword keyword in semanticRoleResult._Object.Keywords)
                                {
                                    model.Add("Palabra clave: " + keyword.Text);
                                }
                            }
                        }
                    }
                }

                if (result.Sentiment != null)
                {
                    if (result.Sentiment.Document != null)
                    {
                        model.Add("Puntaje Sentimiento del documento: " + result.Sentiment.Document.Score);

                        if (result.Sentiment.Targets != null && result.Sentiment.Targets.Count > 0)
                        {
                            foreach (TargetedSentimentResults targetedSentimentResult in result.Sentiment.Targets)
                            {
                                model.Add(string.Format("Resultados de Sentimiento objetivo: {0} | Puntaje: {1}", targetedSentimentResult.Text, targetedSentimentResult.Score));
                            }
                        }
                    }
                }
            }
            else
            {
                model.Add("Resultado es nulo");
            }


            return result;
        }
        #endregion


    }
}
