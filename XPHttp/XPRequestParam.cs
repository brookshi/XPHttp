using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace XPHttp
{
    public class XPRequestParam
    {
        public IHttpContent Body { get; set; }

        public HttpContentHeaderCollection Headers { get; } = new HttpContentHeaderCollection();

        public Dictionary<string, string> QueryStrings { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> UrlSegments { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> Cookies { get; } = new Dictionary<string, string>();


        public XPRequestParam Builder()
        {
            return new XPRequestParam();
        }

        public XPRequestParam AddCookie(string name, string value)
        {
            Cookies[name] = value;
            return this;
        }

        public XPRequestParam AddHeader(string name, string value)
        {
            Headers.Append(name, value);
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

        public XPRequestParam SetBody(IHttpContent body)
        {
            Body = body;
            return this;
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
