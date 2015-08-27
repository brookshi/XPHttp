#region License
//   Copyright 2015 Brook Shi
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 
#endregion

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace XPHttp.HttpFilter
{
    public class HttpRetryFilter : ICustomHttpFilter
    {
        private const string RETRIES = "_retries_";

        public IHttpFilter InnerFilter { get; set; }

        public int RetryTimes { get; set; }

        public IList<HttpStatusCode> RetryHttpCodes { get; set; }

        public HttpRetryFilter(int retryTimes, IList<HttpStatusCode> retryHttpCodes)
        {
            RetryTimes = Math.Max(0, retryTimes);
            RetryHttpCodes = retryHttpCodes;
        }

        public HttpRetryFilter(int retryTimes) : this(retryTimes, new HttpStatusCode[] { HttpStatusCode.ServiceUnavailable })
        {
        }

        public HttpRetryFilter() : this(0, new HttpStatusCode[] { HttpStatusCode.ServiceUnavailable })
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
                    return await SendRequestAsync(request.Clone());
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
            var retries = 1;
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
