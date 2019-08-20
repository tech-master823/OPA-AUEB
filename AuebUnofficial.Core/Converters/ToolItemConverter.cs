using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EclassApi.Models;
using Flurl.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AuebUnofficial.Core.Converters
{
    public class ToolItemConverter : JsonConverter
    {
        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var item = JObject.Load(reader);
            if (Enum.TryParse(item["Type"].Value<string>(), out ToolType toolType))
            {
                var name = item["Name"].Value<string>();
                var link = item["Link"].Value<string>();
                var tool = new ToolBase()
                {
                    Name =  name,
                    Link = link,
                    Type = toolType
                };
                switch (toolType)
                {
                    case ToolType.announcements:
                        var content = item["Content"].Last.First;
                        
                        return new AnnouncementsTool(tool){Content = content.HasValues ? JsonConvert.DeserializeObject<List<Announcement>>(content.ToString()) : new List<Announcement>()};
                    case ToolType.coursedescription:
                        return new CourseDescriptionTool(tool){Content = item["Content"].Value<string>()};
                    case ToolType.description:
                        return new DescriptionTool(tool){Content = item["Content"].Value<string>() };
                    case ToolType.docs:
                        return new DocsTool(tool){RootDirectory = item["RootDirectory"].Value<string>(), RootDirectoryDownloadLink = item["RootDirectoryDownloadLink"].Value<string>()};
                }

                return item.ToObject<ToolBase>();
            }

            return item.ToObject<ToolBase>();


        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ToolBase).IsAssignableFrom(objectType);
        }
    }
}
