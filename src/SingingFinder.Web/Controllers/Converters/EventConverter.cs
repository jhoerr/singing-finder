using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SingingFinder.Core;

namespace SingingFinder.Web.Controllers.Converters
{
    public class EventConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Event);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var item = (Event)value;
            writer.WriteValue(item.Singing);
            writer.WriteValue(item.Days.ToList());
            writer.Flush();
        }
    }
}