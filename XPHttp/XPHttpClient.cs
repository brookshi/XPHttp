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
        public static readonly XPHttpClient DefaultClient = new XPHttpClient();

        private HttpClient _httpClient;

        private HttpRetryFilter _httpRetryFilter;

        public XPHttpClientConfig HttpConfig { get; private set; }

        public XPRequestParam RequestParamBuilder {
            get
            {
                return new XPRequestParam();
            }
        }

        public XPHttpClient()
        {
            _httpRetryFilter = new HttpRetryFilter();
            _httpClient = new HttpClient(_httpRetryFilter);
            HttpConfig = new XPHttpClientConfig(_httpClient.DefaultRequestHeaders, ApplyConfig);
            ApplyConfig();
        }

        void ApplyConfig()
        {
            if (HttpConfig.CustomHttpFilter != null)
            {
                _httpRetryFilter.InnerFilter = HttpConfig.CustomHttpFilter;
            }

            _httpRetryFilter.RetryTimes = HttpConfig.RetryTimes;
            _httpRetryFilter.RetryHttpCodes = HttpConfig.HttpStatusCodesForRetry;
        }

        string BuildUrl(string functionUrl, XPRequestParam param)
        {
            var url = HttpConfig.BaseUrl + functionUrl;
            if (param != null)
            {
                foreach (var segment in param.UrlSegments)
                {
                    url = url.Replace("{" + segment.Key + "}", segment.Value.UrlEncoding());
                }

                foreach (var queryString in param.QueryStrings)
                {
                    url = url.AppendQueryString(queryString);
                }
            }

            return url;
        }

        void ConfigRequest(HttpRequestMessage request, XPRequestParam httpParam)
        {
            if (httpParam == null)
                return;

            request.Content = httpParam.Body;
            foreach(var header in httpParam.Headers)
            {
                request.Headers.Append(header.Key, header.Value);
            }
        }

        public void GetAsync(string functionUrl, XPRequestParam httpParam, IResponseHandler responseHandler)
        {
            SendRequestAsync(HttpMethod.Get, functionUrl, httpParam, responseHandler);
        }

        public void PostAsync(string functionUrl, XPRequestParam httpParam, IResponseHandler responseHandler)
        {
            SendRequestAsync(HttpMethod.Get, functionUrl, httpParam, responseHandler);
        }

        public void PutAsync(string functionUrl, XPRequestParam httpParam, IResponseHandler responseHandler)
        {
            SendRequestAsync(HttpMethod.Get, functionUrl, httpParam, responseHandler);
        }

        public void DeleteAsync(string functionUrl, XPRequestParam httpParam, IResponseHandler responseHandler)
        {
            SendRequestAsync(HttpMethod.Get, functionUrl, httpParam, responseHandler);
        }

        public void PatchAsync(string functionUrl, XPRequestParam httpParam, IResponseHandler responseHandler)
        {
            SendRequestAsync(HttpMethod.Get, functionUrl, httpParam, responseHandler);
        }

        public async void SendRequestAsync(HttpMethod httpMethod, string functionUrl, XPRequestParam httpParam, IResponseHandler responseHandler)
        {
            HttpRequestMessage request = new HttpRequestMessage(httpMethod, new Uri(BuildUrl(functionUrl, httpParam)));

            ConfigRequest(request, httpParam);

            IProgress<HttpProgress> progress = new Progress<HttpProgress>(p=> { responseHandler.OnProgress(p); });

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            if (HttpConfig.TimeOut != int.MaxValue && HttpConfig.TimeOut > 0)
            {
                cancellationTokenSource.CancelAfter(HttpConfig.TimeOut * 1000);
            }

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendRequestAsync(request).AsTask(cancellationTokenSource.Token, progress);
                responseHandler.Handle(response);
            }
            catch (TaskCanceledException)
            {
                responseHandler.OnCancel(request);
                return;
            }
            catch(Exception)
            {
                throw new Exception("send request error");
            }
        }
    }
}
