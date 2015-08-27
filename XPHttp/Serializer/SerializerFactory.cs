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
using System.Linq;

namespace XPHttp.Serializer
{
    public static class SerializerFactory
    {
        static Dictionary<string, ISerializer> _dataSerializerMap = new Dictionary<string, ISerializer>();
        static object _lockObj = new object();

        static SerializerFactory()
        {
            SetSerializer("application/json", new SimpleJsonSerializer());
            SetSerializer("text/json", new SimpleJsonSerializer());
            SetSerializer("text/x-json", new SimpleJsonSerializer());
            SetSerializer("text/javascript", new SimpleJsonSerializer());
            SetSerializer("*+json", new SimpleJsonSerializer());
            SetSerializer("*", new SimpleJsonSerializer());
            //SetSerializer("application/xml", new XmlSerializer());
            //SetSerializer("text/xml", new XmlSerializer());
            //SetSerializer("*+xml", new XmlSerializer());
        }

        public static ISerializer GetSerializer(string contentType)
        {
            lock (_lockObj)
            {
                if (_dataSerializerMap.ContainsKey(contentType))
                {
                    return _dataSerializerMap[contentType];
                }
            }

            return null;
        }

        public static void SetSerializer(string contentType, ISerializer serializer)
        {
            lock (_lockObj)
            {
                _dataSerializerMap[contentType] = serializer;
            }
        }

        public static void ReplaceSerializer(Type type, ISerializer serializer)
        {
            lock (_lockObj)
            {
                var keys = _dataSerializerMap.Keys.ToList();
                foreach (var key in keys)
                {
                    var originSerializer = _dataSerializerMap[key];
                    if (originSerializer.GetType() == type)
                    {
                        _dataSerializerMap[key] = serializer;
                    }
                }
            }
        }
    }
}
