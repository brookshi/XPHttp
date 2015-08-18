using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace XPHttp
{
    public class HttpRetryFilter : IHttpFilter
    {
        private IHttpFilter _innerFilter;
        public HttpRetryFilter(IHttpFilter innerFilter)
        {
            if(innerFilter == null)
            {
                throw new ArgumentNullException("innerFilter cannot be null.");
            }
            _innerFilter = innerFilter;
        }

        public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(HttpRequestMessage request)
        {
            return AsyncInfo.Run<HttpRequestMessage, HttpProgress>(async (cancellationToken, progress) =>
            {
                HttpResponseMessage response = await _innerFilter.SendRequestAsync(request).AsTask(cancellationToken, progress);

                return response;
            });
        }

        public void Dispose()
        {
            _innerFilter.Dispose();
        }
    }
}
