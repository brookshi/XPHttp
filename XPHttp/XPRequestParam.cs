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

using System.Collections.Generic;
using Windows.Data.Json;
using Windows.Storage.Streams;
using Windows.Web.Http;
using XPHttp.HttpContent;

namespace XPHttp
{
    public class XPRequestParam
    {
        public IHttpContent Body { get; set; }

        public string Authorization { get; set; }

        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> QueryStrings { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> UrlSegments { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> Cookies { get; } = new Dictionary<string, string>();

        public XPRequestParam AddCookie(string key, string value)
        {
            Cookies[key] = value;
            return this;
        }

        public XPRequestParam AddHeader(string key, string value)
        {
            Headers[key] = value;
            return this;
        }

        public XPRequestParam AddHeader(params string[] keyValues)
        {
            Headers.AppendForDict<string>(keyValues);
            return this;
        }

        public XPRequestParam AddHeader(string[] keys, string[] values)
        {
            Headers.AppendForDict<string, string>(keys, values);
            return this;
        }

        public XPRequestParam AddQueryString(string key, string value)
        {
            QueryStrings[key] = value;
            return this;
        }

        public XPRequestParam AddQueryString(params string[] keyValues)
        {
            QueryStrings.AppendForDict<string>(keyValues);
            return this;
        }

        public XPRequestParam AddQueryString(string[] keys, string[] values)
        {
            QueryStrings.AppendForDict<string, string>(keys, values);
            return this;
        }

        public XPRequestParam AddUrlSegements(string key, string value)
        {
            UrlSegments[key] = value;
            return this;
        }

        public XPRequestParam AddUrlSegements(params string[] keyValues)
        {
            UrlSegments.AppendForDict<string>(keyValues);
            return this;
        }

        public XPRequestParam AddUrlSegements(string[] keys, string[] values)
        {
            UrlSegments.AppendForDict<string, string>(keys, values);
            return this;
        }

        public XPRequestParam SetObjectBody(object obj, HttpContentType contentType)
        {
            return SetBody(HttpContentFactory.BuildHttpContent(contentType, obj));
        }

        public XPRequestParam SetBody(IHttpContent body)
        {
            Body = body;
            return this;
        }

        public XPRequestParam SetJsonObjectBody(IJsonValue jsonValue)
        {
            return SetBody(new HttpJsonContent(jsonValue));
        }

        public XPRequestParam SetJsonStringBody(string jsonValue)
        {
            return SetBody(new HttpJsonContent(jsonValue));
        }

        public XPRequestParam SetStringBody(string body)
        {
            Body = new HttpStringContent(body);
            return this;
        }

        public XPRequestParam SetStreamBody(IInputStream body)
        {
            Body = new HttpStreamContent(body);
            return this;
        }

        public XPRequestParam SetAuthorization(string authorization)
        {
            Authorization = authorization;
            return this;
        }
    }
}
