using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http.Filters;

namespace XPHttp.HttpFilter
{
    public interface ICustomHttpFilter : IHttpFilter
    {
        IHttpFilter InnerFilter { get; set; }
    }
}
