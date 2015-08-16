using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPHttp
{
    public class XPHttpParam
    {
        public string Body { get; set; }

        public object BodyObj { get; set; }

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

        public XPHttpParam SetBodyString(string body)
        {
            Body = body;
            return this;
        }

        public XPHttpParam SetBodyObject(object body)
        {
            BodyObj = body;
            return this;
        }
    }
}
