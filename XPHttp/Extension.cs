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
using System.Linq;
using System.Text;
using Windows.Web.Http;

namespace XPHttp
{
    public static class Extension
    {
        public static string UrlEncoding(this string url)
        {
            if(url == null)
            {
                throw new ArgumentNullException();
            }

            return Uri.EscapeDataString(url);
        }

        public static string AppendQueryString(this string url, KeyValuePair<string, string> queryStrings)
        {
            if (url == null)
            {
                throw new ArgumentNullException();
            }

            StringBuilder urlBuilder = new StringBuilder(url.Trim().Trim("&?".ToCharArray()).Trim());

            urlBuilder.Append(url.Contains('?') ? "&" : "?");
            urlBuilder.Append(queryStrings.BuildStringForKeyValue());

            return urlBuilder.ToString();
        }

        public static string BuildStringForKeyValue(this KeyValuePair<string, string> keyValue)
        {
            return string.Format("{0}={1}", keyValue.Key, keyValue.Value.UrlEncoding());
        }

        public static void AppendForDict<T1, T2>(this Dictionary<T1, T2> dict, T1[] keys, T2[] values)
        {
            if (keys.Length != values.Length)
                throw new ArgumentException();

            for (int i = 0; i < keys.Length; i++)
            {
                dict[keys[i]] = values[i];
            }
        }

        public static void AppendForDict<T>(this Dictionary<T, T> dict, T[] keyValues)
        {
            if (keyValues.Length % 2 != 0)
                throw new ArgumentException();

            for (int i = 0; i < keyValues.Length; i += 2)
            {
                dict[keyValues[i]] = keyValues[i + 1];
            }
        }

        public static HttpRequestMessage Clone(this HttpRequestMessage originRequest)
        {
            var request = new HttpRequestMessage(originRequest.Method, originRequest.RequestUri);

            foreach(var header in originRequest.Headers)
            {
                request.Headers.TryAppendWithoutValidation(header.Key, header.Value);
            }

            foreach(var property in originRequest.Properties)
            {
                request.Properties.Add(property);
            }

            request.Content = originRequest.Content;

            return request;
        }
    }
}
