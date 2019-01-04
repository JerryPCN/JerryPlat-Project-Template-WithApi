using JerryPlat.Utils.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace JerryPlat.Utils.Helpers
{
    public class HttpHelper
    {
        //public delegate TResult ResponseAsyncDelegate<TResult>(HttpResponseMessage response) where TResult : class;

        //public delegate TResult AjaxAsyncDelegate<TResult, TPostData>(HttpClient client, ResponseAsyncDelegate<TResult> responseAsyncDelegate) where TResult : class;

        #region Owin
        public void SetDefaultAuthorizationHeaderValue(ref AuthenticationHeaderValue authenticationHeaderValue)
        {
            if (authenticationHeaderValue == null)
            {
                authenticationHeaderValue = GetAuthenticationHeaderValue();
            }
        }
        public virtual string GetBaseUrl()
        {
            return string.Empty;
        }

        protected virtual AuthenticationHeaderValue GetAuthenticationHeaderValue()
        {
            return null;
        }
        protected virtual AuthenticationHeaderValue GetAuthenticationHeaderValue(string schema, string parameter)
        {
            return new AuthenticationHeaderValue(schema, parameter);
        }

        protected T GetResponseResult<T>(string strResponse)
        {
            ResponseModel<T> result = JsonConvert.DeserializeObject<ResponseModel<T>>(strResponse);
            if (result == null || result.Status != ConstantHelper.Ok)
            {
                return default(T);
            }
            return result.Data;
        }

        //protected T GetJContainerResult<T>(string strResponse) where T : JContainer
        //{
        //    JObject result = (JObject)JsonConvert.DeserializeObject(strResponse);
        //    if (result == null || (string)result["Status"] != "Success")
        //    {
        //        return default(T);
        //    }
        //    return (T)result["Data"];
        //}

        public TResult AjaxAsync<TResult, TPostData>(string url, TPostData jsonData, RequestType requestType, 
            Func<HttpClient, Func<HttpResponseMessage, TResult>, TResult> ajaxAsyncDelegate, 
            AuthenticationHeaderValue authenticationHeaderValue = null)
            where TResult : class
        {
            SetDefaultAuthorizationHeaderValue(ref authenticationHeaderValue);

            using (HttpClient client = SingleInstanceHelper.GetInstance<HttpClient>())
            {
                client.BaseAddress = new Uri(GetBaseUrl());

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (authenticationHeaderValue != null)
                {
                    client.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
                }

                return ajaxAsyncDelegate(client, response =>
                {
                    string strResponseBody = response.Content.ReadAsStringAsync().Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return SerializationHelper.JsonToObject<TResult>(strResponseBody);
                    }
                    else
                    {
                        LogHelper.Info(strResponseBody);
                    }
                    return default(TResult);
                });
            }
        }

        public TResult GetAsync<TResult>(string strUrl, AuthenticationHeaderValue authenticationHeaderValue = null)
             where TResult : class
        {
            return AjaxAsync<TResult, string>(strUrl, string.Empty, RequestType.None, (client, responseAsync) =>
            {
                using (HttpResponseMessage response = client.GetAsync(strUrl).Result)
                {
                    return responseAsync(response);
                }
            }, authenticationHeaderValue);
        }

        public void GetAsync<TResult>(string strUrl, Action<TResult> callback, AuthenticationHeaderValue authenticationHeaderValue = null)
            where TResult : JContainer
        {
            string strResponse = GetAsync<string>(strUrl, authenticationHeaderValue);
            TResult responseResult = GetResponseResult<TResult>(strResponse);
            if (responseResult == null)
            {
                return;
            }
            callback(responseResult);
        }

        public TResult PostAsync<TResult, TPostData>(string strUrl, TPostData postData, RequestType requestType, AuthenticationHeaderValue authenticationHeaderValue = null)
            where TResult : class
        {
            return AjaxAsync<TResult, TPostData>(strUrl, postData, requestType, (client, responseAsync) =>
            {
                HttpContent content;

                switch (requestType)
                {
                    case RequestType.Form:
                        //content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        content = new FormUrlEncodedContent(postData as IEnumerable<KeyValuePair<string, string>>);
                        break;

                    case RequestType.Json:
                    default:
                        string postJsonData = string.Empty;
                        if (postData is object)
                        {
                            postJsonData = SerializationHelper.ToJson(postData);
                        }
                        else
                        {
                            postJsonData = postData.ToString();
                        }
                        content = new StringContent(postJsonData, Encoding.UTF8);
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        break;
                }

                using (HttpResponseMessage response = client.PostAsync(strUrl, content).Result)
                {
                    return responseAsync(response);
                }
            }, authenticationHeaderValue);
        }

        public void PostAsync<TResult, TPostData>(string strUrl, TPostData postData, RequestType requestType, Action<TResult> callback, AuthenticationHeaderValue authenticationHeaderValue = null)
            where TResult : JContainer
        {
            string strResponse = PostAsync<string, TPostData>(strUrl, postData, requestType, authenticationHeaderValue);
            TResult responseResult = GetResponseResult<TResult>(strResponse);
            if (responseResult == null)
            {
                return;
            }
            callback(responseResult);
        }
        #endregion



        #region URL请求数据

        public static T Post<T>(string url, string param)
            where T : class
        {
            return SerializationHelper.JsonToObject<T>(Post(url, param));
        }

        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string Post(string url, string param)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }

            return responseStr;
        }

        public static T Get<T>(string url)
            where T : class
        {
            return SerializationHelper.JsonToObject<T>(Get(url));
        }

        public static Stream GetStream(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 15000;
            request.Proxy = null;
            request.Method = "GET";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3");
            request.UserAgent = "Mozilla/5.0 (Windows NT 5.2; rv:12.0) Gecko/20100101 Firefox/12.0";

            WebResponse response = null;
            Stream responseStream = null;

            try
            {
                response = request.GetResponse();

                if (response != null)
                {
                    responseStream = response.GetResponseStream();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                throw;
            }
            finally
            {
                request = null;
                response = null;
            }

            return responseStream;
        }

        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string Get(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            WebResponse response = null;
            string responseStr = null;

            try
            {
                response = request.GetResponse();

                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                throw;
            }
            finally
            {
                request = null;
                response = null;
            }

            return responseStr;
        }

        /// <summary>
        /// 执行URL获取页面内容
        /// </summary>
        public static string UrlExecute(string urlPath)
        {
            if (string.IsNullOrEmpty(urlPath))
            {
                return "error";
            }
            StringWriter sw = new StringWriter();
            try
            {
                HttpContext.Current.Server.Execute(urlPath, sw);
                return sw.ToString();
            }
            catch (Exception)
            {
                return "error";
            }
            finally
            {
                sw.Close();
                sw.Dispose();
            }
        }

        #endregion URL请求数据

        #region LocalDoｍain
        public static string GetLocalDomain()
        {
            string scheme = HttpContext.Current.Request.Url.Scheme;
            string authority = HttpContext.Current.Request.Url.Authority;
            return scheme + "://" + authority;
        }
        public static bool IsLocalUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            return (((url[0] == '/') && ((url.Length == 1) || ((url[1] != '/') && (url[1] != '\\')))) || (((url.Length > 1) && (url[0] == '~')) && (url[1] == '/')))
                || url.StartsWith(GetLocalDomain());
        }
        #endregion

    }
}