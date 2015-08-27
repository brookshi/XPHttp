#region License
//   Copyright 2015 Brook Shi
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 
#endregion

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
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
        private static Dictionary<Type, dynamic> _propertyCache = new Dictionary<Type, dynamic>();

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
            result = GetMember(binder.Name);

            return true;
        }

        public override string ToString()
        {
            return _jsonObj.Stringify();
        }

        object GetMember(string name)
        {
            IJsonValue value = null;
            if (_jsonObj.TryGetValue(name, out value))
            {
                return Convert(value);
            }

            return null;
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
