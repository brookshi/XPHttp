using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace XPHttp.Serializer
{
    public interface ISerializer
    {
        string Serialize(object obj);

        T Deserialize<T>(string content);
    }
}
