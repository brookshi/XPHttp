﻿#region License
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
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using XPHttp.HttpFilter;

namespace XPHttp
{
    public class XPHttpClientConfig
    {
        public XPHttpClientConfig(XPHttpClient xpHttpClient, HttpClient httpClient, ICustomHttpFilter retryHttpFilter, Action applyConfig)
        {
            _client = xpHttpClient;
            DefaultRequestHeader = httpClient.DefaultRequestHeaders;
            CustomHttpFilter = retryHttpFilter;
            ApplyConfig = applyConfig;
        }

        private XPHttpClient _client;

        private static HttpStatusCode[] _defaultHttStatuspCodeForRetry = { HttpStatusCode.ServiceUnavailable };

        public Action ApplyConfig { get; set; }

        public string BaseUrl { get; set; } = string.Empty;

        public HttpRequestHeaderCollection DefaultRequestHeader { get; set; }

        public int TimeOut { get; set; } = 30;

        public int RetryTimes { get; set; } = 3;

        public List<HttpStatusCode> HttpStatusCodesForRetry { get; set; } = new List<HttpStatusCode>(_defaultHttStatuspCodeForRetry);

        public ICustomHttpFilter CustomHttpFilter { get; private set; }

        public UnicodeEncoding? ContentEncoding { get; set; } = null;

        public string MediaType { get; set; } = null;

        public XPHttpClientConfig SetContentEncoding(UnicodeEncoding encoding)
        {
            ContentEncoding = encoding;
            return this;
        }

        public XPHttpClientConfig SetMediaType(string mediaType)
        {
            MediaType = mediaType;
            return this;
        }

        public XPHttpClientConfig SetUseHttpCache(bool bUse)
        {
            _client.SetCache(bUse);
            return this;
        }

        public XPHttpClientConfig SetBaseUrl(string baseUrl)
        {
            BaseUrl = baseUrl;
            return this;
        }

        public XPHttpClientConfig SetDefaultHeaders(string name, string value)
        {
            DefaultRequestHeader.Append(name, value);
            return this;
        }

        public XPHttpClientConfig SetDefaultHeaders(params string[] nameValue)
        {
            if (nameValue.Length % 2 != 0)
                throw new ArgumentException();

            for (int i = 0; i < nameValue.Length; i += 2)
            {
                DefaultRequestHeader.Append(nameValue[i], nameValue[i + 1]);
            }
            return this;
        }

        public XPHttpClientConfig SetDefaultHeaders(string[] names, string[] values)
        {
            if (names.Length != values.Length)
                throw new ArgumentException();

            for (int i = 0; i < names.Length; i++)
            {
                DefaultRequestHeader.Append(names[i], values[i]);
            }
            return this;
        }

        public XPHttpClientConfig SetAuthorization(string scheme, string authorization)
        {
            DefaultRequestHeader.Authorization = new HttpCredentialsHeaderValue(scheme, authorization);
            return this;
        }

        public XPHttpClientConfig SetCookeie(string name, string value)
        {
            DefaultRequestHeader.Cookie.Add(new HttpCookiePairHeaderValue(name, value));
            return this;
        }

        public XPHttpClientConfig SetTimeOut(int timeOutSec)
        {
            TimeOut = timeOutSec;
            return this;
        }

        public XPHttpClientConfig SetRetryTimes(int retryTimes)
        {
            RetryTimes = retryTimes;
            return this;
        }

        public XPHttpClientConfig AddRetryStatusCode(HttpStatusCode statusCode)
        {
            HttpStatusCodesForRetry.Add(statusCode);
            return this;
        }

        public XPHttpClientConfig AppendHttpFilter(ICustomHttpFilter httpFilter)
        {
            var tempFilter = CustomHttpFilter;
            tempFilter.InnerFilter = httpFilter;
            CustomHttpFilter = httpFilter;
            return this;
        }
    }
}
