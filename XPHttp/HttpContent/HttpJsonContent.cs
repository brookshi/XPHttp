using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using XPHttp.Serializer;

namespace XPHttp.HttpContent
{
    public class HttpJsonContent : IHttpContent
    {
        private string _jsonValue;

        private HttpContentHeaderCollection _headers = new HttpContentHeaderCollection();

        private ISerializer _serializer = SerializerFactory.GetSerializer("application/json");

        private IBuffer _buffer;

        private InMemoryRandomAccessStream _stream;

        public IBuffer Buffer
        {
            get
            {
                if(_buffer == null)
                {
                    InitBufferInfo();
                }
                return _buffer;
            }
        }

        public InMemoryRandomAccessStream Stream
        {
            get
            {
                if(_stream == null)
                {
                    InitStreamInfo();
                }
                return _stream;
            }
        }

        void InitBufferInfo()
        {
            DataWriter writer = new DataWriter();
            writer.WriteString(_jsonValue);
            _buffer = writer.DetachBuffer();
        }

        async void InitStreamInfo()
        {
            _stream = new InMemoryRandomAccessStream();

            DataWriter writer = new DataWriter(_stream);
            writer.WriteString(_jsonValue);

            var data = await writer.StoreAsync();
            writer.DetachStream();
        }

        public HttpContentHeaderCollection Headers
        {
            get
            {
                return _headers;
            }
        }

        public HttpJsonContent(IJsonValue jsonData)
        {
            if (jsonData == null)
            {
                throw new ArgumentException("jsonData cannot be null.");
            }
            InitData(jsonData.Stringify());
        }

        public HttpJsonContent(object jsonData)
        {
            if (jsonData == null)
            {
                throw new ArgumentException("jsonData cannot be null.");
            }
            InitData(_serializer.Serialize(jsonData));
        }

        public HttpJsonContent(object jsonData, ISerializer serializer)
        {
            if (jsonData == null)
            {
                throw new ArgumentException("jsonData cannot be null.");
            }
            _serializer = serializer;
            InitData(_serializer.Serialize(jsonData));
        }

        public HttpJsonContent(string jsonValue)
        {
            InitData(jsonValue);
        }

        void InitData(string jsonValue)
        {
            this._jsonValue = jsonValue;
            _headers = new HttpContentHeaderCollection();
            _headers.ContentType = new HttpMediaTypeHeaderValue("application/json");
            _headers.ContentType.CharSet = "UTF-8";
        }

        public IAsyncOperationWithProgress<ulong, ulong> BufferAllAsync()
        {
            return AsyncInfo.Run<ulong, ulong>((cancellationToken, progress) => 
            {
                return Task<ulong>.Run(() => 
                {
                    progress.Report(Buffer.Length);
                    return (ulong)Buffer.Length;
                });
            });
        }

        public IAsyncOperationWithProgress<IBuffer, ulong> ReadAsBufferAsync()
        {
            return AsyncInfo.Run<IBuffer, ulong>((cancellationToken, progress) => 
            {
                return Task<IBuffer>.Run(() => 
                {
                    progress.Report(Buffer.Length);
                    return Buffer;
                });
            });
        }

        public IAsyncOperationWithProgress<IInputStream, ulong> ReadAsInputStreamAsync()
        {
            return AsyncInfo.Run<IInputStream, ulong>((cancellationToken, progress) =>
            {
                return Task<IInputStream>.Run(() =>
                {
                    progress.Report(Stream.Size);
                    return Stream.GetInputStreamAt(0);
                });
            });
        }

        public IAsyncOperationWithProgress<string, ulong> ReadAsStringAsync()
        {
            return AsyncInfo.Run<string, ulong>((cancellationToken, progress) =>
            {
                return Task<string>.Run(() =>
                {
                    progress.Report((ulong)_jsonValue.Length);
                    return _jsonValue;
                });
            });
        }

        public bool TryComputeLength(out ulong length)
        {
            length = Buffer.Length;
            return true;
        }

        public IAsyncOperationWithProgress<ulong, ulong> WriteToStreamAsync(IOutputStream outputStream)
        {
            return AsyncInfo.Run<ulong, ulong>(async (cancellationToken, progress) =>
            {
                DataWriter writer = new DataWriter(outputStream);
                writer.WriteString(_jsonValue);
                var size = await writer.StoreAsync();
                writer.DetachStream();

                progress.Report(size);
                return size;
            });
        }

        public void Dispose()
        {
            _buffer = null;
        }
    }
}
