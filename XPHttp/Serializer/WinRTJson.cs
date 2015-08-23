using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace XPHttp.Serializer
{
    public class WinRTJson
    {
        public static Task<dynamic> DeserializeAsync(string json)
        {
            return Task.Run(() =>
            {
                return Deserialize(json);
            });
        }

        public static Task<dynamic> DeserializeAsync(JsonObject json)
        {
            return Task.Run(() =>
            {
                return Deserialize(json);
            });
        }

        public static dynamic Deserialize(string json)
        {
            return new JsonToDynamic(json);
        }

        public static dynamic Deserialize(JsonObject json)
        {
            return new JsonToDynamic(json);
        }

        public static string Serialize(object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(obj.GetType());
                serializer.WriteObject(stream, obj);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }

    public class JsonToDynamic : DynamicObject
    {
        private readonly JsonObject _jsonObj;

        public JsonToDynamic(string json)
        {
            _jsonObj = JsonObject.Parse(json);
        }

        public JsonToDynamic(JsonObject json)
        {
            _jsonObj = json;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _jsonObj.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            IJsonValue value = null;
            if(_jsonObj.TryGetValue(binder.Name, out value))
            {
                result = Convert(value);
            }
            else
            {
                result = null;
            }
            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            var type = binder.Type;
            //TODO: binding property.
            result = null;
            return true;
        }

        public override string ToString()
        {
            return _jsonObj.Stringify();
        }

        dynamic Convert(IJsonValue json)
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
                    obj = new JsonToDynamic(json.GetObject());
                    break;
                case JsonValueType.String:
                    obj = json.GetString();
                    break;
            }
            return obj;
        }

        dynamic ConvertArray(JsonArray jsonArray)
        {
            return jsonArray.Select(Convert).ToArray();
        }
    }
}
