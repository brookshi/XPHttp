using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace XPHttp
{
    public class XPHttpClient
    {
        public static readonly XPHttpClient Instance = new XPHttpClient();

        private HttpClient _httpClient;

        private IHttpFilter _httpFilter;

        public XPHttpClient()
        {
        }

        public void Init(XPHttpConfig config)
        {

        }
    }
}
