using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace XPHttp
{
    public class XPHttpParam
    {
        public IHttpContent Body { get; set; }

        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> QueryStrings { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> UrlSegments { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> Cookies { get; } = new Dictionary<string, string>();


        public XPHttpParam Builder()
        {
            return new XPHttpParam();
        }

        public XPHttpParam AddCookie(string name, string value)
        {
            Cookies[name] = value;
            return this;
        }

        public XPHttpParam AddHeader(string name, string value)
        {
            Headers[name] = value;
            return this;
        }

        public XPHttpParam AddQueryString(string name, string value)
        {
            QueryStrings[name] = value;
            return this;
        }

        public XPHttpParam AddUrlSegements(string name, string value)
        {
            UrlSegments[name] = value;
            return this;
        }

        public XPHttpParam SetBody(IHttpContent body)
        {
            Body = body;
            return this;
        }

        public XPHttpParam SetStringBody(string body)
        {
            Body = new HttpStringContent(body);
            return this;
        }

        public XPHttpParam SetStreamBody(IInputStream body)
        {
            Body = new HttpStreamContent(body);
            return this;
        }
    }
}
