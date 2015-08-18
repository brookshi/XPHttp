using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace XPHttp
{
    public class XPHttpConfig
    {
        private static HttpStatusCode[] _defaultHttpCodeForRetry = { HttpStatusCode.ServiceUnavailable };

        public string BaseUrl { get; set; }

        public Dictionary<string, string> DefaultHeaders { get; set; } = new Dictionary<string, string>();

        public int TimeOut { get; set; } = 30;

        public int Retries { get; set; } = 3;

        public List<HttpStatusCode> RetryForHttpCodes { get; set; } = new List<HttpStatusCode>(_defaultHttpCodeForRetry);

        public Func<DateTime, string> DateFormatter { get; set; } = dateTime => { return dateTime.ToString("yyyy-MM-dd"); };

        public IHttpFilter CustomHttpFilter { get; set; }

        public static XPHttpConfig Builder { get { return new XPHttpConfig(); } }

        public XPHttpConfig SetBaseUrl(string baseUrl)
        {
            BaseUrl = baseUrl;
            return this;
        }

        public XPHttpConfig SetGlobalHeaders(string name, string value)
        {
            DefaultHeaders[name] = value;
            return this;
        }

        public XPHttpConfig SetTimeOut(int timeOut)
        {
            TimeOut = timeOut;
            return this;
        }

        public XPHttpConfig SetRetryTimes(int retryTimes)
        {
            Retries = retryTimes;
            return this;
        }

        public XPHttpConfig SetDateFormatter(Func<DateTime, string> formatter)
        {
            DateFormatter = formatter;
            return this;
        }

        public XPHttpConfig SetHttpFilter(IHttpFilter httpFilter)
        {
            CustomHttpFilter = httpFilter;
            return this;
        }
    }
}
