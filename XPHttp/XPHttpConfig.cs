using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPHttp
{
    public class XPHttpConfig
    {
        public string BaseUrl { get; set; }
        public Dictionary<string, string> GlobalHeaders { get; set; } = new Dictionary<string, string>();
        public int TimeOut { get; set; } = 30;
        public int RetryTimes { get; set; } = 3;
        public Func<DateTime, string> DateFormatter { get; set; } = dateTime => { return dateTime.ToString("yyyy-MM-dd"); };

        public static XPHttpConfig Builder { get { return new XPHttpConfig(); } }

        public XPHttpConfig SetBaseUrl(string baseUrl)
        {
            BaseUrl = baseUrl;
            return this;
        }

        public XPHttpConfig SetGlobalHeaders(string name, string value)
        {
            GlobalHeaders[name] = value;
            return this;
        }

        public XPHttpConfig SetTimeOut(int timeOut)
        {
            TimeOut = timeOut;
            return this;
        }

        public XPHttpConfig SetRetryTimes(int retryTimes)
        {
            RetryTimes = retryTimes;
            return this;
        }

        public XPHttpConfig SetDateFormatter(Func<DateTime, string> formatter)
        {
            DateFormatter = formatter;
            return this;
        }
    }
}
