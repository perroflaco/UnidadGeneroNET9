using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq.Expressions;

namespace SEV.Library
{
    // Main class
    public class HALEncoder<T>
    {

        private T CurrentState { get; set; }

        private object Payload { get; set; }

        private HALResource ResultState { get; set; }

        public static IConfiguration Configuration { get; set; }

        public HALEncoder()
        {
            SetGlobalConfiguration(SEVConfigAssistant.Configuration);
        }

        public static Boolean IsForcedHTTPSOnCurrentAPI()
        {
            Boolean Result = false;
            String CurrentForceValue = "";
            CurrentForceValue = SEVConfigAssistant.Configuration["EntitiesConfig:Encoder:ForceHTTPSOnCurrentAPI"];
            if(CurrentForceValue != null)
            {
                Result = (CurrentForceValue.ToUpper() == "ON");
            }
            return Result;
        }

        public static void SetGlobalConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public HALResponse<T> FromState(T origin)
        {
            CurrentState = origin;
            Payload = null;
            HALResponse<T> ResultContainer = new HALResponse<T>();
            ResultContainer.SetOrigin(this);
            return ResultContainer;
        }

        public void Build(HALResponse<T> Dest, String Entity, ActionExecutedContext Context)
        {
            Process(Dest, Entity, Context);
        }

        public HALEncoder<T> WhitPayload(object payload)
        {
            Payload = payload;
            return this;
        }

        private void Process(HALResponse<T> Dest, String Entity, ActionExecutedContext Context)
        {
            HALConfigReader CurrentHALConfig = new HALConfigReader(Entity, Context);
            HALResource ResultState = HALResource.Create();
            HALEntitySpects EntitySpect = CurrentHALConfig.RetrieveEntity();

            Type typeList = typeof(List<>);
            Type typeState = CurrentState.GetType().IsGenericType ? CurrentState.GetType().GetGenericTypeDefinition() : CurrentState.GetType().DeclaringType;

            if (typeList.Equals(typeState))
            {
                JObject CurrentPayload = Payload == null ? new JObject() : JObject.FromObject(Payload);
                EntitySpect.ResolveLinks
                        (
                            new JObject(),
                            LinkResult =>
                                ResultState
                                    .AddLink(LinkResult["Key"].ToString())
                                        .WithLink(LinkResult["Value"].ToString()),
                            true
                        );
                ResultState
                    .FromState(CurrentPayload)
                    .AddEmbedded("content");

                JArray StateToProcessList = JArray.FromObject(CurrentState);
                foreach (JObject StateElement in StateToProcessList)
                {
                    HALResource ResourceElement = HALResource.Create().FromState(StateElement);
                    EntitySpect.ResolveEmbedded
                    (
                        StateElement,
                        LinkResult =>
                        (
                            ResourceElement
                                    .AddLink(LinkResult["Key"].ToString())
                                        .WithLink(LinkResult["Value"].ToString())

                        )
                    );
                    ResultState.
                        AddEmbeddedChild(ResourceElement);
                }
            }
            else
            {
                JObject StateToProcess = JObject.FromObject(CurrentState);
                EntitySpect
                    .ResolveLinks
                    (
                        StateToProcess,
                        LinkResult =>
                            ResultState
                                .AddLink(LinkResult["Key"].ToString())
                                    .WithLink(LinkResult["Value"].ToString())

                    );
                JObject StateToProcessObject = JObject.FromObject(CurrentState);
                ResultState.FromState(StateToProcessObject);
            }
            foreach (JProperty ResultProps in ResultState.Properties())
            {
                Dest.Add(ResultProps.Name, ResultProps.Value);
            }
        }
    }

    // Prototype for resolving the result with yelding objects
    public class HALResponseBase : JObject
    {
        public virtual void Build(String Entity, ActionExecutedContext Context) { }
    }

    public class HALResponse<T> : HALResponseBase
    {
        [JsonIgnore]
        private HALEncoder<T> _Encoder { get; set; }

        public HALResponse() { }

        public void SetOrigin(HALEncoder<T> Encoder)
        {
            _Encoder = Encoder;
        }

        public HALResponse<T> WithPayload(object payload)
        {
            _Encoder.WhitPayload(payload);
            return this;
        }

        public override void Build(String Entity, ActionExecutedContext Context)
        {
            _Encoder.Build(this, Entity, Context);
        }
    }

    // Class for config file reading (Singleton Pattern)
    public class HALConfigReader
    {
        // Section Identifiers
        private const String _ENTITIES_CONFIG = "EntitiesConfig";
        private const String _ENTITIES = "Entities";
        private const String _GLOBAL_STRINGS = "GlobalStrings";
        public const String _CURRENT_REQUEST_LABEL = "_REQUEST_";
        public const String _CURRENT_SERVER_ROOT_LABEL = "_SERVER_ROOT_";
        public const String _CURRENT_PROTOCOL_LABEL = "_PROTOCOL_";
        public const String _CURRENT_QUERYSTRING_LABEL = "_QUERYSTRING_";
        public const String _CURRENT_PATH_LABEL = "_PATH_";

        // Global configuration
        private static IConfiguration Configuration { get; set; }

        // Global Containers
        private static List<String> ValidEntities { get; set; }
        private static List<HALGlobalStrings> GlobalStrings { get; set; }
        private List<HALGlobalStrings> RequestStrings { get; set; }

        // Object Entity to work name
        private String CurrentEntity { get; set; }
        private ActionExecutedContext Context { get; set; }

        public HALConfigReader(String entity, ActionExecutedContext context)
        {
            CurrentEntity = entity;
            Context = context;
            RequestStrings = new List<HALGlobalStrings>();
            SetAPIRequestStrings();
        }

        public static void SetConfiguration(IConfiguration config)
        {
            ValidEntities = new List<String>();
            GlobalStrings = new List<HALGlobalStrings>();
            Configuration = config;
            GetConfiguration();
            GetGlobalStrings();
        }

        private static void GetConfiguration()
        {
            IConfigurationSection ConfigSection = Configuration.GetSection(_ENTITIES_CONFIG);
            foreach (String Entity in ConfigSection.GetSection(_ENTITIES).Get<string[]>())
            {
                ValidEntities.Add(Entity);
            }
        }

        private static void GetGlobalStrings()
        {
            IConfigurationSection GlobalStringsSection = Configuration.GetSection(_ENTITIES_CONFIG);
            foreach (HALGlobalStrings GlobalString in GlobalStringsSection.GetSection(_GLOBAL_STRINGS).Get<HALGlobalStrings[]>())
            {
                GlobalStrings.Add(GlobalString);
            }
        }

        private HALConfigReader SetAPIRequestStrings()
        {
            var CurrentRequest = Context.HttpContext.Request;
            char SLASH_LABEL = '/';
            String CurrentScheme = HALEncoder<Object>.IsForcedHTTPSOnCurrentAPI() ? "https" : CurrentRequest.Scheme;
            String CurrentHost = CurrentRequest.Host.Value;
            String CurrentPath = CurrentRequest.Path.Value.EndsWith("/") ? CurrentRequest.Path.Value.TrimEnd(SLASH_LABEL) : CurrentRequest.Path.Value;
            String CurrentQueryString = CurrentRequest.QueryString.HasValue ? CurrentRequest.QueryString.Value : "";
            RequestStrings.Add(new HALGlobalStrings() { Id = _CURRENT_REQUEST_LABEL, Value = String.Format("{0}://{1}{2}{3}", CurrentScheme, CurrentHost, CurrentPath, CurrentQueryString) });
            RequestStrings.Add(new HALGlobalStrings() { Id = _CURRENT_SERVER_ROOT_LABEL, Value = String.Format("{0}://{1}", CurrentScheme, CurrentHost) });
            RequestStrings.Add(new HALGlobalStrings() { Id = _CURRENT_PROTOCOL_LABEL, Value = CurrentScheme });
            RequestStrings.Add(new HALGlobalStrings() { Id = _CURRENT_QUERYSTRING_LABEL, Value = CurrentQueryString });
            RequestStrings.Add(new HALGlobalStrings() { Id = _CURRENT_PATH_LABEL, Value = CurrentPath });
            return this;
        }

        public String SolveGlobalStrings(String LinkValue)
        {
            String Result = LinkValue;
            GlobalStrings.ForEach
            (
                GlobalString =>
                Result = Result.Replace(GlobalString.Id.ToTemplated(), GlobalString.Value)
            );
            return Result;
        }

        public String SolveRequestStrings(String LinkValue)
        {
            String Result = LinkValue;
            RequestStrings.ForEach
            (
                RequestString =>
                Result = Result.Replace(RequestString.Id.ToTemplated(), RequestString.Value)
            );
            return Result;
        }

        public HALEntitySpects RetrieveEntity()
        {
            return new HALEntitySpects(this, Configuration, CurrentEntity + "Entity");
        }
    }

    public class HALEntitySpects : JObject
    {
        public const String SELF_LABEL = "self";
        public const String LINKS_LABEL = "_links";
        public const String EMBEDDED_LABEL = "_embedded";

        private HALConfigReader Reader { get; set; }
        private IConfiguration LoadedSection { get; set; }
        private String SectionToLoad { get; set; }

        public HALEntitySpects(HALConfigReader reader, IConfiguration config, String section)
        {
            Reader = reader;
            LoadedSection = config;
            SectionToLoad = section;
            LoadLinksFromConfig();
            LoadEmdeddedFormConfig();
        }

        private void LoadEmdeddedFormConfig()
        {
            JObject Embedded = new JObject();
            JObject EmbeddedLinksFromConfig = new JObject();
            HALEntityDefinition EmbeddedLinks = new HALEntityDefinition();
            string SectionToFind = SectionToLoad + ":_embedded";
            HALEntityDefinition EmbeddedItems = new HALEntityDefinition();
            LoadedSection.GetSection(SectionToFind).Bind(EmbeddedItems);
            foreach (var SectionLinkItem in EmbeddedItems._links)
            {
                EmbeddedLinksFromConfig.Add(SectionLinkItem.Key, JObject.FromObject(SectionLinkItem.Value));
            }
            Embedded.Add(LINKS_LABEL, EmbeddedLinksFromConfig);
            Add(EMBEDDED_LABEL, Embedded);
        }

        private void LoadLinksFromConfig()
        {
            JObject Links = new JObject();
            string SectionToFind = SectionToLoad;
            HALEntityDefinition LinkItems = new HALEntityDefinition();
            LoadedSection.GetSection(SectionToFind).Bind(LinkItems);
            if (!LinkItems._links.ContainsKey(SELF_LABEL))
            {
                HalEntityLinkEntry LinkDefinition = new HalEntityLinkEntry()
                {
                    templatedHref = HALConfigReader._CURRENT_REQUEST_LABEL.ToTemplated(),
                    applyWhenEmbedded = true
                };
                Links.Add(SELF_LABEL, JObject.FromObject(LinkDefinition));
            }
            foreach (var SectionLinkItem in LinkItems._links)
            {
                Links.Add(SectionLinkItem.Key, JObject.FromObject(SectionLinkItem.Value));
            }

            Add(LINKS_LABEL, Links);
        }

        public HALEntitySpects ResolveLinks(JObject Item, Expression<Func<JObject, Object>> func, Boolean IsEmbeddedPresent = false)
        {
            JObject Links = (JObject)this[LINKS_LABEL];
            if (Links != null)
            {
                foreach (JProperty CurrentProperty in Links.Properties())
                {
                    JObject LinkItem = (JObject)CurrentProperty.Value;
                    String CurrentValue = Reader.SolveRequestStrings(LinkItem["templatedHref"].ToString());
                    CurrentValue = Reader.SolveGlobalStrings(CurrentValue);
                    foreach (JProperty CurrentItem in Item.Properties())
                    {
                        CurrentValue = CurrentValue.Replace(CurrentItem.Name.ToString().ToTemplated(), CurrentItem.Value.ToString());
                    }
                    JObject Result = new JObject();
                    Result.Add("Key", CurrentProperty.Name.ToString());
                    Result.Add("Value", CurrentValue);
                    Boolean MustApplyOnEmbed = (Boolean)LinkItem["applyWhenEmbedded"];
                    Boolean MustOnlyOnEmbed = (Boolean)LinkItem["onlyWhenEmbedded"];
                    if (!(IsEmbeddedPresent && !MustApplyOnEmbed))
                    {
                        func.Compile()(Result);
                    }
                }
            }
            return this;
        }

        public HALEntitySpects ResolveEmbedded(JObject Item, Expression<Func<JObject, Object>> func)
        {
            JObject Embedded = (JObject) this[EMBEDDED_LABEL];
            JObject Links = (JObject) Embedded[LINKS_LABEL];
            if (Links != null)
            {
                foreach (JProperty CurrentProperty in Links.Properties())
                {
                    JObject LinkItem = (JObject)CurrentProperty.Value;
                    String CurrentValue = Reader.SolveRequestStrings(LinkItem["templatedHref"].ToString());
                    CurrentValue = Reader.SolveGlobalStrings(CurrentValue);
                    foreach (JProperty CurrentItem in Item.Properties())
                    {
                        CurrentValue = CurrentValue.Replace(CurrentItem.Name.ToString().ToTemplated(), CurrentItem.Value.ToString());
                    }
                    JObject Result = new JObject();
                    Result.Add("Key", CurrentProperty.Name.ToString());
                    Result.Add("Value", CurrentValue);
                    func.Compile()(Result);
                }
            }
            return this;
        }
    }

    //Super class for defining resources
    public class HALConfigResource
    {
    }

    // Hal config implementation
    public class HALConfig : HALConfigResource
    {
        public string[] Entities { get; set; }
        public HALGlobalStrings[] GlobalStrings { get; set; }
    }

    // Definition for Entity
    public class HALEntityDefinition
    {
        public Dictionary<String, HalEntityLinkEntry> _links { get; set; }
        public Dictionary<String, HALEntityDefinition> _embedded { get; set; }

        public HALEntityDefinition()
        {
            _links = new Dictionary<string, HalEntityLinkEntry>();
            _embedded = new Dictionary<string, HALEntityDefinition>();
        }
    }

    public class HalEntityLinkEntry
    {
        public String templatedHref { get; set; }
        public Boolean applyWhenEmbedded { get; set; } = false;
        public Boolean onlyWhenEmbedded { get; set; } = false;
    }

    public class HALGlobalStrings
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
}
