XPHttp
--------
--------

An asynchronous, callback-based Http client for UWP (Universal Windows Platform)

Features
--------
- Only supports Universal Windows Platform
- Automatic XML and JSON serialization/deserialization
- Supports custom serialization and deserialization via ISerializer and IDeserializer
- friendly **params builder** (RequestParams)
- Automatic smart **request retries** optimized for spotty mobile connections
- Supports RESTful API(GET, POST, PUT, DELETE, PATCH)

RoadMap
--------
- Supports gzip
- Supports download/upload files
- Supports oAuth


How To Use
--------

- ###init http client:
<pre><code>
XPHttpClient.DefaultClient.HttpConfig.SetBaseUrl("http://news-at.zhihu.com/api/4/") //base url
                .SetDefaultHeaders("Host", "news-at.zhihu.com", "UserAgent","123") //global header
                .SetTimeOut(45) // time out (second, default is 30)
                .SetRetryTimes(3) // retry times (default is 3)
                .AddRetryStatusCode(HttpStatusCode.MethodNotAllowed) // http status code for retry (default is ServiceUnavailable)
                .AppendHttpFilter(new MyHttpFilter()) //custom http filter
                .ApplyConfig(); // apply to http client
</code></pre>
    			
- ###init request param:
<pre><code>
var reqParam = XPHttpClient.DefaultClient.RequestParamBuilder.AddHeader("referer", "gugugu", "UserAgent", "321") // request header
                .AddUrlSegements("action", "get", "date", "latest"); // url segments, replace {action} and {date} to "get" and "latest" in url
</code></pre>
				
- ###request data:
<pre><code>
XPHttpClient.DefaultClient.GetAsync("stories/{date}" /*function url */, reqParam, new XPResponseHandler<dynamic>() { // callback
                OnCancel = requestMsg => { txt_cancel.Text = "cancel"; },
                OnFinish = async responseMsg => { txt_finish.Text = "finish: " + await responseMsg.Content.ReadAsStringAsync(); },
                OnFailed = async responseMsg => { txt_failed.Text = "failed: " + await responseMsg.Content.ReadAsStringAsync(); },
                OnProgress = progress => { txt_cancel.Text += progress.Stage.ToString(); },
                OnSuccess = async (responseMsg, obj) => { txt_success.Text = "success: " + await responseMsg.Content.ReadAsStringAsync() + "\r\n"+obj.stories[0].id; },
            });
</code></pre>