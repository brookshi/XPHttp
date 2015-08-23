using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace XPHttp.Serializer
{
    public class JsonParser
    {
        public static Task<dynamic> ParseAsync(string json)
        {
            return Task.Run(() =>
            {
                return Parse(json);
            });
        }

        public static Task<dynamic> ParseAsync(IJsonValue json)
        {
            return Task.Run(() =>
            {
                return Parse(json);
            });
        }

        public static dynamic Parse(string json)
        {
            var j = JsonValue.Parse(json);
            return Parse(j);
        }

        public static dynamic Parse(IJsonValue json)
        {
            dynamic obj = Convert(json);
            return obj;
        }

        static dynamic Convert(IJsonValue json)
        {
            dynamic obj = null;
            switch (json.ValueType)
            {
                case JsonValueType.Array:
                    obj = ConvertArray(json.GetArray());
                    break;
                case JsonValueType.Boolean:
                    obj = json.GetBoolean();
                    break;
                case JsonValueType.Null:
                    obj = null;
                    break;
                case JsonValueType.Number:
                    obj = json.GetNumber();
                    break;
                case JsonValueType.Object:
                    obj = ConvertObject(json.GetObject());
                    break;
                case JsonValueType.String:
                    obj = json.GetString();
                    break;
            }
            return obj;
        }

        static dynamic ConvertArray(JsonArray jsonArray)
        {
            dynamic[] items = new dynamic[jsonArray.Count];
            for (int i = 0; i < jsonArray.Count; i++)
            {
                items[i] = Convert(jsonArray[i]);
            }
            return items;
        }

        static dynamic ConvertObject(JsonObject jsonObject)
        {
            dynamic obj = new ExpandoObject();
            var d = (IDictionary<string, object>)obj;

            obj.Contains = new Func<string, bool>((prop) =>
            {
                return d.ContainsKey(prop);
            });
            obj.Get = new Func<string, dynamic>((prop) =>
            {
                return d[prop];
            });

            List<string> keys = new List<string>();
            foreach (var key in jsonObject.Keys)
            {
                keys.Add(key);
            }

            int i = 0;
            foreach (var item in jsonObject.Values)
            {
                d.Add(keys[i], Convert(item));
                i++;
            }
            return obj;
        }
    }
}
