using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

namespace SEV.Library
{
    public class HALResource : JObject
    {
        [JsonIgnore]
        private JObject _LastLink { get; set; }
        [JsonIgnore]
        private JArray _CurrentEmbed { get; set; }

        public JObject _embedded { get; set; }

        public HALResource()
        {
            _LastLink = null;
            Add("_links", new JObject());
        }


        public static HALResource Create()
        {
            HALResource Result = new HALResource();
            return Result;

        }

        public HALResource FromState(JObject State)
        {
            foreach(JProperty StateProperty in State.Properties())
            {
                Add(StateProperty.Name, StateProperty.Value);
            }
            return this;
        }

        public HALResource AddLink(String Label)
        {
            _LastLink = new HALLink();
            JObject LinkProp = (JObject) this["_links"];
            LinkProp.Add(Label, _LastLink);
            return this;
        }

        public HALResource AddSelfLink()
        {
            AddLink("self");
            return this;
        }

        public HALResource WithLink(string URL)
        {
            if (_LastLink != null) {
                _LastLink.Add("href", URL);
            }
            return this;
        }

        public HALResource AddEmbedded(String Label)
        {
            JProperty Embedded = Property("_embedded");
            if (Embedded == null)
            {
                JObject EmbeddedContainer = new JObject();
                _CurrentEmbed = new JArray();
                EmbeddedContainer.Add(Label, _CurrentEmbed);
                Add("_embedded", EmbeddedContainer);
            }
            else
            {
                JObject EmbeddedProp = (JObject)this["_embedded"];
                if (EmbeddedProp.Property(Label) == null) 
                {
                    _CurrentEmbed = new JArray();
                    EmbeddedProp.Add(Label, _CurrentEmbed);
                }
                else
                {
                    JToken EmbedFinded = null;
                    if (EmbeddedProp.TryGetValue(Label, out EmbedFinded))
                    {
                        _CurrentEmbed = EmbedFinded.Value<JArray>();
                    }
                    else
                    {
                        _CurrentEmbed = new JArray();
                        EmbeddedProp.Add(Label, _CurrentEmbed);
                    }
                }
            }
            return this;
        }

        public HALResource AddEmbeddedChild(HALResource Child)
        {
            if(_CurrentEmbed!= null)
            {
                _CurrentEmbed.Add(Child);
            }
            return this;
        }
    }

    public class HALLink : JObject
    {
        public string rel { get; set; }
        public string href { get; set; }
        public string title { get; set; }
        public bool isTemplated { get; set; }

        
    }
}
