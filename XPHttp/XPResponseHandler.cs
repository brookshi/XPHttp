using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
