using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace XPHttp.HttpContent
{
    public static class HttpContentFactory
    {
        public static IHttpContent BuildHttpContent(HttpContentType type, object data)
        {
            switch(type)
            {
                case HttpContentType.Json:
                    return new HttpJsonContent(data);
                case HttpContentType.Xml:
                    return null;//TODO: add xml serializer
                case HttpContentType.Text:
                    return new HttpStringContent(data.ToString());
                default:
                    return null;
            }
        }
    }
}
