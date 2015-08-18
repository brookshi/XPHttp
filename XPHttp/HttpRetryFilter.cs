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
        private int _retryTimes = 0;
        private readonly IList<HttpStatusCode> _retryHttpCodes;

        public HttpRetryFilter(IHttpFilter innerFilter, int retryTimes, IList<HttpStatusCode> retryHttpCodes)
        {
            if(innerFilter == null)
            {
                throw new ArgumentNullException("innerFilter cannot be null.");
            }

            _innerFilter = innerFilter;
            _retryTimes = Math.Max(0, retryTimes);
            _retryHttpCodes = retryHttpCodes;
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
