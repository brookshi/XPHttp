using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace XPHttp
{
    public interface IResponseHandler
    {
        Action<HttpResponseMessage, object> OnSuccess { get; set; }

        Action<HttpResponseMessage> OnFailed { get; set; }

        Action<HttpResponseMessage> OnFinish { get; set; }

        Action<HttpProgress> OnProgress { get; set; }

        Action<HttpRequestMessage> OnCancel { get; set; }

        void Handle(HttpResponseMessage response);
    }

    public interface IResponseHandler<T> : IResponseHandler
    {
        new Action<HttpResponseMessage, T> OnSuccess { get; set; }
    }
}
