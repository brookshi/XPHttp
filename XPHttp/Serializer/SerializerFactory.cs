using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPHttp.Serializer
{
    public static class SerializerFactory
    {
        static Dictionary<string, ISerializer> _dataSerializerMap = new Dictionary<string, ISerializer>();
        static object _lockObj = new object();

        static SerializerFactory()
        {
            SetSerializer("application/json", new JsonSerializer());
            SetSerializer("text/json", new JsonSerializer());
            SetSerializer("text/x-json", new JsonSerializer());
            SetSerializer("text/javascript", new JsonSerializer());
            SetSerializer("*+json", new JsonSerializer());
            SetSerializer("*", new JsonSerializer());
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
