using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace XPHttp
{
    public class HttpRetryFilter : IHttpFilter
    {
        private const string RETRIES = "_retries_";
        private IHttpFilter _innerFilter;
        private int _retryTimes;
        private IList<HttpStatusCode> _retryHttpCodes;

        public HttpRetryFilter(XPHttpConfig config)
        {
            if(config == null)
            {
                SetDefaultRetryProperty();
                return;
            }

            var innerFilter = config.CustomHttpFilter;
            if (innerFilter == null)
            {
                innerFilter = new HttpBaseProtocolFilter();
            }

            _innerFilter = innerFilter;
            _retryTimes = Math.Max(0, config.RetryTimes);
            _retryHttpCodes = config.RetryForHttpStatusCodes;
        }

        void SetDefaultRetryProperty()
        {
            _innerFilter = new HttpBaseProtocolFilter();
            _retryTimes = 0;
            _retryHttpCodes = new List<HttpStatusCode>();
        }

        public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(HttpRequestMessage request)
        {
            return AsyncInfo.Run<HttpResponseMessage, HttpProgress>(async (cancellationToken, progress) =>
            {
                HttpResponseMessage response = await _innerFilter.SendRequestAsync(request).AsTask(cancellationToken, progress);

                cancellationToken.ThrowIfCancellationRequested();
                
                if (_retryHttpCodes != null && _retryHttpCodes.Contains(response.StatusCode) && GetRetries(request) < _retryTimes)
                {
                    IncreaseRetries(request);
                    return await SendRequestAsync(request);
                }
                else
                {
                    return response;
                }
            });
        }

        int GetRetries(HttpRequestMessage request)
        {
            if(!request.Properties.ContainsKey(RETRIES))
            {
                return 0;
            }
            return int.Parse(request.Properties[RETRIES].ToString());
        }

        void IncreaseRetries(HttpRequestMessage request)
        {
            var retries = 0;
            if (request.Properties.ContainsKey(RETRIES))
            {
                retries = int.Parse(request.Properties[RETRIES].ToString()) + 1;
            }
            request.Properties[RETRIES] = retries;
        }

        public void Dispose()
        {
            _innerFilter.Dispose();
        }
    }
}
