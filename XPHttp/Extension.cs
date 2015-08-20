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
        public static string UrlEncoding(this string url)
        {
            if(url == null)
            {
                throw new ArgumentNullException();
            }

            return Uri.EscapeDataString(url);
        }

        public static string AppendQueryString(this string url, KeyValuePair<string, string> queryStrings)
        {
            if (url == null)
            {
                throw new ArgumentNullException();
            }

            StringBuilder urlBuilder = new StringBuilder(url.Trim().Trim("&?".ToCharArray()).Trim());

            urlBuilder.Append(url.Contains('?') ? "&" : "?");
            urlBuilder.Append(queryStrings.BuildStringForKeyValue());

            return urlBuilder.ToString();
        }

        public static string BuildStringForKeyValue(this KeyValuePair<string, string> keyValue)
        {
            return string.Format("{0}={1}", keyValue.Key, keyValue.Value);
        }
    }
}
