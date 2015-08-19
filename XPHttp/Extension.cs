using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace XPHttp
{
    public static class Extension
    {
        public static HttpRequestMessage Clone(this HttpRequestMessage originRequest)
        {
            var request = new HttpRequestMessage(originRequest.Method, originRequest.RequestUri);
            return request;
        }

        public static string UrlEncoding(this string url)
        {
            if(url == null)
            {
                throw new ArgumentNullException();
            }

            return Uri.EscapeDataString(url);
        }
    }
}
