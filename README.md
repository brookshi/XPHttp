#XPHttp
=======

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
XPHttpClient.DefaultClient.HttpConfig.SetBaseUrl("http://news-at.zhihu.com/api/4/") <div style="color:green">//base url</div>
                .SetDefaultHeaders("Host", "news-at.zhihu.com", "UserAgent","123") <div style="color:green">//global header</div>
                .SetTimeOut(45) <div style="color:green">// time out (second, default is 30)</div>
                .SetRetryTimes(3) <div style="color:green">// retry times (default is 3)</div>
                .AddRetryStatusCode(HttpStatusCode.MethodNotAllowed) <div style="color:green">// http status code for retry (default is ServiceUnavailable)</div>
                .AppendHttpFilter(new MyHttpFilter()) <div style="color:green">//custom http filter</div>
                .ApplyConfig(); <div style="color:green">// apply to http client</div>
</code></pre>
    			
- ###init request param:
<pre><code>
var reqParam = XPHttpClient.DefaultClient.RequestParamBuilder.AddHeader("referer", "gugugu", "UserAgent", "321") <div style="color:green">// request header</div>
                .AddUrlSegements("action", "get", "date", "latest"); <div style="color:green">// url segments, replace {action} and {date} to "get" and "latest" in url</div>
</code></pre>
				
- ###request data:
<pre><code>
XPHttpClient.DefaultClient.GetAsync("stories/{date}" <div style="color:green">/*function url */</div>, reqParam, new XPResponseHandler<dynamic>() { <div style="color:green">// callback</div>
                OnCancel = requestMsg => { txt_cancel.Text = "cancel"; },
                OnFinish = async responseMsg => { txt_finish.Text = "finish: " + await responseMsg.Content.ReadAsStringAsync(); },
                OnFailed = async responseMsg => { txt_failed.Text = "failed: " + await responseMsg.Content.ReadAsStringAsync(); },
                OnProgress = progress => { txt_cancel.Text += progress.Stage.ToString(); },
                OnSuccess = async (responseMsg, obj) => { txt_success.Text = "success: " + await responseMsg.Content.ReadAsStringAsync() + "\r\n"+obj.stories[0].id; },
            });
</code></pre>

License
=======
<pre><code>
Copyright 2015 Brook Shi

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. 
</code></pre>