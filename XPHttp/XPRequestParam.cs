using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using XPHttp.HttpContent;
using XPHttp.Serializer;

namespace XPHttp
{
    public class XPRequestParam
    {
        public IHttpContent Body { get; set; }

        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> QueryStrings { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> UrlSegments { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> Cookies { get; } = new Dictionary<string, string>();

        public XPRequestParam AddCookie(string name, string value)
        {
            Cookies[name] = value;
            return this;
        }

        public XPRequestParam AddHeader(string name, string value)
        {
            Headers[name] = value;
            return this;
        }

        public XPRequestParam AddQueryString(string name, string value)
        {
            QueryStrings[name] = value;
            return this;
        }

        public XPRequestParam AddUrlSegements(string name, string value)
        {
            UrlSegments[name] = value;
            return this;
        }

        public XPRequestParam SetObjectBody(object obj, HttpContentType contentType)
        {
            return SetBody(HttpContentFactory.BuildHttpContent(contentType, obj));
        }

        public XPRequestParam SetBody(IHttpContent body)
        {
            Body = body;
            return this;
        }

        public XPRequestParam SetJsonObjectBody(IJsonValue jsonValue)
        {
            return SetBody(new HttpJsonContent(jsonValue));
        }

        public XPRequestParam SetJsonStringBody(string jsonValue)
        {
            return SetBody(new HttpJsonContent(jsonValue));
        }

        public XPRequestParam SetStringBody(string body)
        {
            Body = new HttpStringContent(body);
            return this;
        }

        public XPRequestParam SetStreamBody(IInputStream body)
        {
            Body = new HttpStreamContent(body);
            return this;
        }
    }
}
