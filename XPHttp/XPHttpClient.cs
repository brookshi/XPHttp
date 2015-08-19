using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace XPHttp
{
    public class XPHttpClient
    {
        public static readonly XPHttpClient Instance = new XPHttpClient();

        private HttpClient _httpClient;

        private IHttpFilter _httpRetryFilter;

        private CancellationTokenSource _cancellationTokenSource;

        private XPHttpConfig HttpConfig {
            get { return XPHttpConfig.Builder; }
        }

        public XPHttpClient()
        {
        }

        public void Init(XPHttpConfig config)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            if (config.TimeOut > 0)
            {
                _cancellationTokenSource.CancelAfter(config.TimeOut * 1000);
            }
            _httpRetryFilter = new HttpRetryFilter(config);
            _httpClient = new HttpClient(_httpRetryFilter);
            foreach (var header in config.DefaultHeaders)
            {
                _httpClient.DefaultRequestHeaders.Add(header);
            }
        }

        string BuildUrl(string functionUrl, XPHttpParam param)
        {
            var url = HttpConfig.BaseUrl + functionUrl;
            foreach(var segment in param.UrlSegments)
            {
                url = url.Replace("{" + segment.Key + "}", segment.Value.UrlEncoding());
            }



            return url;
        }

        public void GetAsync(string functionUrl, XPHttpParam param, IHttpResponseHandler responseHandler)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "");
        }
    }
}
