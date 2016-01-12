using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using XPHttp;
using XPHttp.HttpContent;
using XPHttp.HttpFilter;
using XPHttp.Serializer;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Sample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            //XPHttpClient.DefaultClient.HttpConfig.SetBaseUrl("http://news-at.zhihu.com/api/4/")
            //    .SetDefaultHeaders("Host", "news-at.zhihu.com", "UserAgent", "123")
            //    .SetAuthorization("123", "456")
            //    .SetCookeie("cookie1", "cookie1 value")
            //    .SetCookeie("cookie2", "cookie2 value")
            //    .SetTimeOut(45)
            //    .SetRetryTimes(3)
            //    .SetUseHttpCache(true)
            //    .SetContentEncoding(Windows.Storage.Streams.UnicodeEncoding.Utf8)
            //    .SetMediaType("application/x-www-form-urlencoded")
            //    .AddRetryStatusCode(HttpStatusCode.MethodNotAllowed)
            //    .AppendHttpFilter(new MyHttpFilter())
            //    .ApplyConfig();

            XPHttpClient.DefaultClient.HttpConfig
                .SetBaseUrl("http://news-at.zhihu.com/api/4/")
                .SetUseHttpCache(false)
                .ApplyConfig();
        }

        public void Get()
        {
            var reqParam = XPHttpClient.DefaultClient.RequestParamBuilder.AddHeader("referer", "gugugu", "UserAgent", "321")
                .AddUrlSegements("action", "get", "date", "latest");

            XPHttpClient.DefaultClient.GetAsync("stories/{date}", reqParam, new XPResponseHandler<RootObject>() {
                OnCancel = requestMsg => { txt_cancel.Text = "cancel"; },
                OnFailed = async responseMsg => { txt_failed.Text = "failed: " + await responseMsg.Content.ReadAsStringAsync(); },
                OnSuccess = async (responseMsg, obj) => { txt_success.Text = "success: " + await responseMsg.Content.ReadAsStringAsync() + "\r\n"+obj.stories[0].id; },
            });
        }

        public async Task GetTask()
        {
            NewtonsoftJsonSerializer.SetDateFormats("yyyy-MM-dd");
            var reqParam = XPHttpClient.DefaultClient.RequestParamBuilder.AddHeader("referer", "gugugu", "UserAgent", "321")
                .SetAuthorization("000", "789")
                .AddUrlSegements("action", "get", "date", "latest");

            var obj = await XPHttpClient.DefaultClient.GetAsync<RootObject>("stories/{date}", reqParam);
            if (obj == null)
                txt_success.Text = "failed";
            else
                txt_success.Text = "success: " + obj.date + "\r\n" + obj.stories[0].id;
        }

        public Task<T> GetLatest<T>()
        {
            var httpParam = XPHttpClient.DefaultClient.RequestParamBuilder
               .AddUrlSegements("storyid", "");

            return XPHttpClient.DefaultClient.GetAsync<T>("stories/latest", httpParam);
        }

        public async void PostWithTask()
        {
            var reqParam = XPHttpClient.DefaultClient.RequestParamBuilder.AddHeader("referer", "gugugu", "UserAgent", "321")
               .AddUrlSegements("action", "get", "date", "latest")
               .SetContentEncoding(Windows.Storage.Streams.UnicodeEncoding.Utf16BE)
               .SetIfModifiedSince(DateTime.Now)
               .SetStringBody("data=1");
            await XPHttpClient.DefaultClient.PostAsync("stories/{date}", reqParam);
            Debug.Write("data response");
        }

        public void Post()
        {
            var reqParam = XPHttpClient.DefaultClient.RequestParamBuilder.AddHeader("referer", "gugugu", "UserAgent", "321")
                .AddUrlSegements("action", "get", "date", "latest")
                .SetContentEncoding(Windows.Storage.Streams.UnicodeEncoding.Utf16BE)
                //.SetMediaType("Application/json")
                .SetIfModifiedSince(DateTime.Now)
                .SetStringBody("data=1");
                //.SetBody(new HttpJsonContent(new { a="a", b=DateTime.Now }));

            XPHttpClient.DefaultClient.PostAsync("stories/{date}", reqParam, new XPResponseHandler<RootObject>()
            {
                OnCancel = requestMsg => { txt_cancel.Text = "cancel"; },
                OnFinish = async responseMsg => { txt_finish.Text = "finish: " + await responseMsg.Content.ReadAsStringAsync(); },
                OnFailed = async responseMsg => { txt_failed.Text = "failed: " + await responseMsg.Content.ReadAsStringAsync(); },
                OnProgress = progress => { txt_cancel.Text += progress.Stage.ToString(); },
                OnSuccess = async (responseMsg, obj) => { txt_success.Text = "success: " + await responseMsg.Content.ReadAsStringAsync() + "\r\n" + obj.stories[0].id; },
            });
        }

        private async void Get_Click(object sender, RoutedEventArgs e)
        {
            //Get();
            var obj = await GetLatest<RootObject>();
            txt_success.Text = obj.date;
        }

        private void Post_Click(object sender, RoutedEventArgs e)
        {
            //Post();
            PostWithTask();
        }
    }

    public class MyHttpFilter : ICustomHttpFilter
    {
        public IHttpFilter InnerFilter
        {
            get; set;
        }

        public void Dispose()
        {
            
        }

        public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(HttpRequestMessage request)
        {
            Debug.WriteLine("--------> MyHttpFilter");
            return AsyncInfo.Run<HttpResponseMessage, HttpProgress>(async (cancelToken,progress)=>await InnerFilter.SendRequestAsync(request).AsTask());
        }
    }

    public class Story
    {
        public Story(){}

        public List<string> images { get; set; }
        public int type { get; set; }
        public int id { get; set; }
        public string ga_prefix { get; set; }
        public string title { get; set; }
        public bool? multipic { get; set; }
    }

    public class TopStory
    {
        public TopStory() { }
        public string image { get; set; }
        public int type { get; set; }
        public int id { get; set; }
        public string ga_prefix { get; set; }
        public string title { get; set; }
    }

    public class RootObject
    {
        public RootObject() { }
        public string date { get; set; }
        public List<Story> stories { get; set; }
        public List<TopStory> top_stories { get; set; }
    }
}
