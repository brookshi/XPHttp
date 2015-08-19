using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace XPHttp
{
    public abstract class XPHttpResponseHandlerBase
    {
        public Action<HttpResponseMessage, object> OnSuccess { get; set; }

        public Action<HttpResponseMessage> OnFailed { get; set; }

        public Action<HttpResponseMessage> OnCancel { get; set; }

        public Action<HttpResponseMessage> OnRetry { get; set; }

        public Action<HttpResponseMessage> OnFinish { get; set; }

        public Action<HttpResponseMessage, HttpProgress> OnProgress { get; set; }
    }

    public class XPHttpResponseHandler<T> : XPHttpResponseHandlerBase, IHttpResponseHandler<T>
    {
        public new Action<HttpResponseMessage, T> OnSuccess { get; set; }
    }

    public class XPHttpResponseHandler : XPHttpResponseHandlerBase, IHttpResponseHandler
    { }
}
