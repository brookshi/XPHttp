#region License
//   Copyright 2015 Brook Shi
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 
#endregion

using Windows.Web.Http;

namespace XPHttp.HttpContent
{
    public static class HttpContentFactory
    {
        public static IHttpContent BuildHttpContent(HttpContentType type, object data)
        {
            switch(type)
            {
                case HttpContentType.Json:
                    return new HttpJsonContent(data);
                case HttpContentType.Xml:
                    return null;//TODO: add xml serializer
                case HttpContentType.Text:
                    return new HttpStringContent(data.ToString());
                default:
                    return null;
            }
        }
    }
}
