using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace XPHttp
{
    public interface IHttpResponseHandler
    {
        Action<HttpResponseMessage, object> OnSuccess { get; set; }
        Action<HttpResponseMessage> OnFailed { get; set; }
        Action<HttpResponseMessage> OnCancel { get; set; }
        Action<HttpResponseMessage> OnRetry { get; set; }
        Action<HttpResponseMessage> OnFinish { get; set; }
        Action<HttpResponseMessage, HttpProgress> OnProgress { get; set; }
    }

    public interface IHttpResponseHandler<T> : IHttpResponseHandler
    {
        new Action<HttpResponseMessage, T> OnSuccess { get; set; }
    }
}
