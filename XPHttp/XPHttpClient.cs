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

        private IHttpFilter _httpFilter;

        private CancellationTokenSource _cancellationTokenSource;

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
            _httpClient = new HttpClient(config.CustomHttpFilter);
            foreach (var header in config.DefaultHeaders)
            {
                _httpClient.DefaultRequestHeaders.Add(header);
            }
        }
    }
}
