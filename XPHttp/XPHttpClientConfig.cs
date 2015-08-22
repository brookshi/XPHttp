using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.Web.Http.Headers;

namespace XPHttp
{
    public class XPHttpClientConfig
    {
        public XPHttpClientConfig(HttpRequestHeaderCollection defaultHeaders, Action applyConfig)
        {
            DefaultHeader = defaultHeaders;
            ApplyConfig = applyConfig;
        }

        private static HttpStatusCode[] _defaultHttStatuspCodeForRetry = { HttpStatusCode.ServiceUnavailable };

        public Action ApplyConfig { get; set; }

        public string BaseUrl { get; set; } = string.Empty;

        public HttpRequestHeaderCollection DefaultHeader { get; set; }

        public int TimeOut { get; set; } = 30;

        public int RetryTimes { get; set; } = 3;

        public List<HttpStatusCode> HttpStatusCodesForRetry { get; set; } = new List<HttpStatusCode>(_defaultHttStatuspCodeForRetry);

        public Func<DateTime, string> DateFormatter { get; set; } = dateTime => { return dateTime.ToString("yyyy-MM-dd"); };

        public IHttpFilter CustomHttpFilter { get; set; }

        public XPHttpClientConfig SetBaseUrl(string baseUrl)
        {
            BaseUrl = baseUrl;
            return this;
        }

        public XPHttpClientConfig SetDefaultHeaders(string name, string value)
        {
            DefaultHeader.Append(name, value);
            return this;
        }

        public XPHttpClientConfig SetTimeOut(int timeOut)
        {
            TimeOut = timeOut;
            return this;
        }

        public XPHttpClientConfig SetRetryTimes(int retryTimes)
        {
            RetryTimes = retryTimes;
            return this;
        }

        public XPHttpClientConfig SetDateFormatter(Func<DateTime, string> formatter)
        {
            DateFormatter = formatter;
            return this;
        }

        public XPHttpClientConfig SetHttpFilter(IHttpFilter httpFilter)
        {
            CustomHttpFilter = httpFilter;
            return this;
        }
    }
}
