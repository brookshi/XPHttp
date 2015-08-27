using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XPHttp;
using XPHttp.HttpContent;
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
            XPHttpClient.DefaultClient.HttpConfig.SetBaseUrl("http://news-at.zhihu.com/api/4/")
                .SetDefaultHeaders("Host", "news-at.zhihu.com", "UserAgent","123")
                .SetTimeOut(5)
                .SetRetryTimes(3);
        }

        public void Get()
        {
            var reqParam = XPHttpClient.DefaultClient.RequestParamBuilder.AddHeader("referer", "gugugu", "UserAgent", "321")
                .AddUrlSegements("action", "get", "date", "latest");

            XPHttpClient.DefaultClient.GetAsync("stories/{date}", reqParam, new XPResponseHandler<dynamic>() {
                OnCancel = requestMsg => { txt_cancel.Text = "cancel"; },
                OnFinish = async responseMsg => { txt_finish.Text = "finish: " + await responseMsg.Content.ReadAsStringAsync(); },
                OnFailed = async responseMsg => { txt_failed.Text = "failed: " + await responseMsg.Content.ReadAsStringAsync(); },
                OnProgress = progress => { txt_cancel.Text += progress.Stage.ToString(); },
                OnSuccess = async (responseMsg, obj) => { txt_success.Text = "success: " + await responseMsg.Content.ReadAsStringAsync() + "\r\n"+obj.stories[0].id; },
            });
        }

        public void Post()
        {
            SerializerFactory.ReplaceSerializer(typeof(JsonSerializer), new SimpleJsonSerializer());

            var reqParam = XPHttpClient.DefaultClient.RequestParamBuilder.AddHeader("referer", "gugugu", "UserAgent", "321")
                .AddUrlSegements("action", "get", "date", "latest")
                .SetBody(new HttpJsonContent(new { a="a", b=DateTime.Now }));

            XPHttpClient.DefaultClient.PostAsync("stories/{date}", reqParam, new XPResponseHandler<dynamic>()
            {
                OnCancel = requestMsg => { txt_cancel.Text = "cancel"; },
                OnFinish = async responseMsg => { txt_finish.Text = "finish: " + await responseMsg.Content.ReadAsStringAsync(); },
                OnFailed = async responseMsg => { txt_failed.Text = "failed: " + await responseMsg.Content.ReadAsStringAsync(); },
                OnProgress = progress => { txt_cancel.Text += progress.Stage.ToString(); },
                OnSuccess = async (responseMsg, obj) => { txt_success.Text = "success: " + await responseMsg.Content.ReadAsStringAsync() + "\r\n" + obj.stories[0].id; },
            });
        }

        private void Get_Click(object sender, RoutedEventArgs e)
        {
            Get();
        }

        private void Post_Click(object sender, RoutedEventArgs e)
        {
            Post();
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
