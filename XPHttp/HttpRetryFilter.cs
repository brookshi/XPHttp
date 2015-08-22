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

        public IHttpFilter InnerFilter { get; set; }

        public int RetryTimes { get; set; }

        public IList<HttpStatusCode> RetryHttpCodes { get; set; }

        public HttpRetryFilter(IHttpFilter httpFilter, int retryTimes, IList<HttpStatusCode> retryHttpCodes)
        {
            var innerFilter = httpFilter;
            if (innerFilter == null)
            {
                innerFilter = new HttpBaseProtocolFilter();
            }

            InnerFilter = innerFilter;
            RetryTimes = Math.Max(0, retryTimes);
            RetryHttpCodes = retryHttpCodes;
        }

        public HttpRetryFilter(int retryTimes, IList<HttpStatusCode> retryHttpCode) : this(null, retryTimes, retryHttpCode)
        {
        }

        public HttpRetryFilter(int retryTimes) : this(null, retryTimes, new HttpStatusCode[] { HttpStatusCode.ServiceUnavailable })
        {
        }

        public HttpRetryFilter() : this(null, 0, new HttpStatusCode[] { HttpStatusCode.ServiceUnavailable })
        {
        }

        public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(HttpRequestMessage request)
        {
            return AsyncInfo.Run<HttpResponseMessage, HttpProgress>(async (cancellationToken, progress) =>
            {
                HttpResponseMessage response = await InnerFilter.SendRequestAsync(request).AsTask(cancellationToken, progress);

                cancellationToken.ThrowIfCancellationRequested();
                
                if (RetryHttpCodes != null && RetryHttpCodes.Contains(response.StatusCode) && GetRetries(request) < RetryTimes)
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
            InnerFilter.Dispose();
        }
    }
}
