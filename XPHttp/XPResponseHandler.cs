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
using Windows.Web.Http;
using XPHttp.Serializer;

namespace XPHttp
{
    public abstract class XPResponseHandlerBase
    {
        public Action<HttpResponseMessage> OnSuccess { get; set; }

        public Action<HttpRequestMessage> OnCancel { get; set; }

        public Action<HttpResponseMessage> OnFinish { get; set; }

        public Action<HttpResponseMessage> OnFailed { get; set; }

        public Action<HttpProgress> OnProgress { get; set; }

        public virtual void Handle(HttpResponseMessage response)
        {
            ExecIfNotNull(() => OnFinish(response));

            if (response.IsSuccessStatusCode)
            {
                ExecIfNotNull(() => OnSuccess(response));
            }
            else
            {
                ExecIfNotNull(() => OnFailed(response));
            }
        }

        protected void ExecIfNotNull(Action action)
        {
            if (action != null)
                action();
        }
    }

    public class XPResponseHandler<T> : XPResponseHandlerBase, IResponseHandler<T>
    {
        public new Action<HttpResponseMessage, T> OnSuccess { get; set; }

        public override async void Handle(HttpResponseMessage response)
        {
            ExecIfNotNull(() => OnFinish(response));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var serializer = SerializerFactory.GetSerializer(response.Content.Headers.ContentType.MediaType);

                ExecIfNotNull(() => OnSuccess(response, serializer.Deserialize<T>(content)));
            }
            else
            {
                ExecIfNotNull(() => OnFailed(response));
            }
        }
    }

    public class XPResponseHandler : XPResponseHandlerBase, IResponseHandler
    { }
}
