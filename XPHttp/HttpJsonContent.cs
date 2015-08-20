using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace XPHttp
{
    public class HttpJsonContent : IHttpContent
    {
        private IJsonValue _jsonData;

        private HttpContentHeaderCollection _headers = new HttpContentHeaderCollection();

        public HttpContentHeaderCollection Headers
        {
            get
            {
                return _headers;
            }
        }

        public HttpJsonContent(IJsonValue jsonData)
        {
            _jsonData = jsonData;
        }

        public IAsyncOperationWithProgress<ulong, ulong> BufferAllAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IAsyncOperationWithProgress<IBuffer, ulong> ReadAsBufferAsync()
        {
            throw new NotImplementedException();
        }

        public IAsyncOperationWithProgress<IInputStream, ulong> ReadAsInputStreamAsync()
        {
            throw new NotImplementedException();
        }

        public IAsyncOperationWithProgress<string, ulong> ReadAsStringAsync()
        {
            throw new NotImplementedException();
        }

        public bool TryComputeLength(out ulong length)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperationWithProgress<ulong, ulong> WriteToStreamAsync(IOutputStream outputStream)
        {
            throw new NotImplementedException();
        }
    }
}
