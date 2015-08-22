using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace XPHttp
{
    public abstract class XPResponseHandlerBase
    {
        public Action<HttpResponseMessage, object> OnSuccess { get; set; }

        public Action<HttpRequestMessage> OnCancel { get; set; }

        public Action<HttpResponseMessage> OnFinish { get; set; }

        public Action<HttpResponseMessage> OnFailed { get; set; }

        public Action<HttpProgress> OnProgress { get; set; }

        public async void Handle(HttpResponseMessage response)
        {
            ExecIfNotNull(() => OnFinish(response));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ExecIfNotNull(() => OnSuccess(response, content));
            }
            else
            {
                ExecIfNotNull(() => OnFailed(response));
            }
        }

        void ExecIfNotNull(Action action)
        {
            if (action != null)
                action();
        }
    }

    public class XPResponseHandler<T> : XPResponseHandlerBase, IResponseHandler<T>
    {
        public new Action<HttpResponseMessage, T> OnSuccess { get; set; }
    }

    public class XPResponseHandler : XPResponseHandlerBase, IResponseHandler
    { }
}
