using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace XPHttp.Serializer
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    var serializer = new DataContractJsonSerializer(obj.GetType());
            //    serializer.WriteObject(stream, obj);
            //    stream.Position = 0;
            //    using (StreamReader reader = new StreamReader(stream))
            //    {
            //        return reader.ReadToEnd();
            //    }
            //}
            //return SimpleJson.SimpleJson.SerializeObject(obj);
            return WinRTJson.Serialize(obj);
        }

        public T Deserialize<T>(string content)
        {
            //var bytes = Encoding.Unicode.GetBytes(content);
            //using (MemoryStream stream = new MemoryStream(bytes))
            //{
            //    var serializer = new DataContractJsonSerializer(typeof(T));
            //    return (T)serializer.ReadObject(stream);
            //}
            //return SimpleJson.SimpleJson.DeserializeObject<T>(content);
            return WinRTJson.Deserialize(content);
        }
    }
}
