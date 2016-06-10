using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ClaritySoft.Api.Client.ApiObjects;

namespace ClaritySoft.Api.Client.ApiClient
{
    public class ApiReader
    {
        readonly string controllerUri;
        readonly string filterRead;
        protected string FilterRead
        {
            get { return filterRead; }
        }
        public HttpClient CreateClient()
        {
            return CreateClient(null);
        }
        public HttpClient CreateClient(string relUri)
        {
            var client = new HttpClient();
            client.BaseAddress = string.IsNullOrEmpty(relUri) ? this.BaseUri : new Uri(this.BaseUri, relUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
        protected string GetUriFromId(Guid id)
        {
            return GetUriByQuery("id=" + id.ToString());
        }
       
        protected static async Task<ExceptionInfo> ProcessError(HttpResponseMessage saveTaskResult)
        {
            {
                var errorMessage = await saveTaskResult.Content.ReadAsStringAsync();
                //using (var rt = saveTask.Result.Content.ReadAsStringAsync())
                {
                    //rt.Wait();
                    var exceptionInfo = new ExceptionInfo
                    {
                        UserFriendlyError = errorMessage,
                        Response = errorMessage
                    };
                    var statusCodeDescription = string.Format("HTTP {0} {1}", (int)saveTaskResult.StatusCode,saveTaskResult.StatusCode.ToString());
                    var headerLog = new System.Text.StringBuilder("Error while querying: ");
                    if (string.IsNullOrEmpty(exceptionInfo.UserFriendlyError))
                    {
                        exceptionInfo.UserFriendlyError = statusCodeDescription;
                    }
                    else
                    {
                        headerLog.Append(statusCodeDescription);
                        headerLog.Append(' ');
                    }
                    headerLog.Append(exceptionInfo.UserFriendlyError);
                    exceptionInfo.TechnicalError = headerLog.ToString();
                    System.Diagnostics.Trace.TraceError(exceptionInfo.TechnicalError);
                    return exceptionInfo;
                }
            }
        }

        public async Task<ExceptionInfo> PostFile(string relUrl, IEnumerable<KeyValuePair<string, string>> values, byte[] file, string fileKey, string fileName)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            using (var client = CreateClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    if (values != null)
                    {
                        foreach (var keyPair in values)
                        {
                            content.Add(new StringContent(keyPair.Value),
                                EncodeFormValue(keyPair.Key));

                        }
                    }
                    content.Add(new ByteArrayContent(file), EncodeFormValue(fileKey), EncodeFormValue(fileName));
                    using (var postResult = await client.PostAsync(GetUriByQuery(relUrl), content))
                    {
                        if (postResult.IsSuccessStatusCode)
                        {
                            return ExceptionInfo.Success;
                        }
                        return await ProcessError(postResult);
                    }
                }



            }
        }
        static string EncodeFormValue(string value)
        {
            return String.Format("\"{0}\"", value);
        }
        protected static string MakeFilterQueryString(string filterRead)
        {
            if (!string.IsNullOrEmpty(filterRead))
            {
                return "$filter=" + filterRead;
            }
            else
            {
                return "";
            }
        }
        public ApiReader(string controllerUri, string filterRead)
        {
            this.controllerUri = controllerUri;//"api/synctasks";
            this.filterRead = MakeFilterQueryString(filterRead);

            
        }
        public async Task<T> Load<T>()
            where T : class
        {
            return await Load<T>("");
        }
        public async Task<T> Load<T>(string url)
            where T : class
        {
            return await LoadByUrl<T>(GetUriByQuery(url));
        }
        private string lastUrlUsed;
        public string LastUrlUsedToQuery
        {
            get { return lastUrlUsed; }
        }
        protected async Task<T> LoadByUrl<T>(string url)
            where T : class
        {
            using (var client = CreateClient())
            {
                lastUrlUsed = client.BaseAddress==null?url: new Uri(client.BaseAddress, url).ToString();
                System.Diagnostics.Trace.TraceInformation("Querying url: {0}", lastUrlUsed);
                var response = await client.GetAsync(url);
                try
                {
                    return await Read<T>(response, true);
                }
                catch
                {
                    throw;
                }
            }
        }
        public async Task Put<T>(Guid id, T value)
            where T : class
        {
            await Put(null, id, value);
        }

        public async Task Put<T>(string relUrl, Guid id, T value)
            where T:class
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id property must be specified as non-NULL GUID");
            }
            using (var client = CreateClient())
            {
                using (var response = await Put<T>(relUrl, value, id, client))
                {
                    await CheckObjectResponse<T>(response);
                }
            }
        }
        public async Task Put<T>(T value)
            where T : class,IHasId
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            var id = value.Id;
            await Put(id,  value);
            
        }
        public void Post<T,TC>(T value, out TC returnValue)
            where T : class
            where TC : class
        {
            var t = Post<T, TC>(value);
            Task.WhenAll(t);
            returnValue = t.Result;
        }
        public async Task Post<T>(T value)
            where T : class
            
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            using (var client = CreateClient())
            {
                using (var response = await Post<T>(value, client))
                {
                    await CheckObjectResponse<T>(response);
                }
            }
        }
        public async Task<TC> Post<T, TC>(T value)
            where T:class
            where TC : class
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            using (var client = CreateClient())
            {
                using (var response = await Post<T>(value, client))
                {
                    return await Read<TC>(response, true);
                }
            }
        }
        public async Task Delete(Guid id)
        {
            using (var client = CreateClient())
            {
                using (var deleteResult = await client.DeleteAsync(GetUriFromId(id)))
                {
                    if (deleteResult.IsSuccessStatusCode)
                    {
                        return;
                    }
                    (await ProcessError(deleteResult)).ThrowIfError();
                }
                


                
            }
        }
        

        protected async Task CheckObjectResponse<T>(HttpResponseMessage taskResponse)
            where T:class
        {
            await Read<T>(taskResponse,false);
        }
        protected Task<HttpResponseMessage> Post<T>(T value, HttpClient client)
        {
            return client.PostAsJsonAsync<T>(GetUriByQuery(null),value);
        }
        protected Task<HttpResponseMessage> Put<T>(string relUrl, T value, Guid id, HttpClient client)
        {
            var query = "?id="+id.ToString();
            if (!string.IsNullOrEmpty(relUrl))
            {
                query = "/" + relUrl.Trim('/') + query;
            }
            return client.PutAsJsonAsync<T>(GetUriByQuery(query),value);
        }
        static readonly MediaTypeFormatter[] readFormatters
            = new MediaTypeFormatter[] {
                    CreateJsonReadFormatter(),
                    new XmlMediaTypeFormatter()
                    };

        static MediaTypeFormatter CreateJsonReadFormatter()
        {
            var fmt = new JsonMediaTypeFormatter();
            fmt.SerializerSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            fmt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            fmt.SerializerSettings.Error = new EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs>(HandleJsonReadError);
            return fmt;
        }
        static void HandleJsonReadError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e )
        {
            //ignore all for now
            e.ErrorContext.Handled = true;
        }
        public static async Task<T> Read<T>(HttpResponseMessage response)
            where T:class
        {
            return await response.Content.ReadAsAsync<T>(readFormatters);
        }
        public static async Task<T> ReadOrThrow<T>(HttpResponseMessage response)
            where T : class
        {
            return await Read<T>(response, true);
        }
        protected static async Task<T> Read<T>(HttpResponseMessage response, bool performRead)
            where T : class
        {
            
            if (response.IsSuccessStatusCode)
            {
                if (performRead)
                {
                    return await Read<T>(response);
                }
            }
            else
            {
                await ThrowException(response);
                
            }

            return null;
        }
        public static async Task ThrowException(HttpResponseMessage response)
        {
            (await ProcessError(response)).ThrowIfError();
        }
        public async Task<IEnumerable<T>> QueryListByFilter<T>(string filter)
        {
            return await QueryListByFilter<T>(filter, null);
        }
        public async Task<IEnumerable<T>> QueryListByFilter<T>(string filter, string urlParameters)
        {
            string useFilter;
            if (string.IsNullOrEmpty(filter))
            {
                useFilter = FilterRead;
            }
            else if (!string.IsNullOrEmpty(FilterRead))
            {
                useFilter = FilterRead + " and " + filter;
            }
            else
            {
                useFilter = MakeFilterQueryString(filter);
            }
            if (!string.IsNullOrEmpty(urlParameters))
            {
                if (!string.IsNullOrEmpty(useFilter))
                {
                    useFilter += '&';
                }
                useFilter += urlParameters;
            }
            //var result = LoadByUrl<IEnumerable<T>>(GetUriByQuery(useFilter));
            var result = await QueryList<T>(useFilter);
            return result ?? new T[] { };
        }

        public async Task<IEnumerable<T>> QueryList<T>(string query)
        {
            return await LoadByUrl<IEnumerable<T>>(GetUriByQuery(query)) ?? new T[] { };
        }
        public async Task<IEnumerable<T>> QueryList<T>()
        {
            return await QueryListByFilter<T>("");
            //return Load<IEnumerable<T>>(GetUriByQuery(this.FilterRead));
        }
        protected static string ToFilterCriteria(int value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
        protected static string ToFilterCriteria(float value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
        protected static string ToFilterCriteria(decimal value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
        protected static string ToFilterCriteria(string value)
        {
            return "'" + Uri.EscapeDataString(value ?? "").Replace("'", "''") + "'";
        }
        protected static string ToFilterCriteria(bool value)
        {
            return value ? "true" : "false";
        }
      
        protected static string ToFilterCriteria(Guid value)
        {
            return "guid" + ToFilterCriteria(value.ToString());
        }
        public static string BuildFilter(string mask, params object[] parameters)
        {
            for (var idx = 0; idx < parameters.Length; idx++)
            {
                var obj = parameters[idx];
                if (obj == null)
                {
                    obj = ToFilterCriteria("");
                }
                else if (obj is Guid)
                {
                    obj = ToFilterCriteria((Guid)obj);
                }
                else if (obj is DateTime)
                {
                    obj = ToFilterCriteria((DateTime)obj);
                }
                else if (obj is bool)
                {
                    obj = ToFilterCriteria((bool)obj);
                }
                else if (obj is int)
                {
                    obj = ToFilterCriteria((int)obj);
                }
                else if (obj is decimal)
                {
                    obj = ToFilterCriteria((decimal)obj);
                }
                else if (obj is float)
                {
                    obj = ToFilterCriteria((float)obj);
                }
                else
                {
                    obj = ToFilterCriteria(obj.ToString());
                }
                parameters[idx] = obj;
            }
            return string.Format(mask, parameters);

        }
        protected static string ToFilterCriteria(DateTime value)
        {
            return "datetime" + ToFilterCriteria(value.ToString(@"yyyy\-MM\-dd\THH\:mm\:ss", System.Globalization.CultureInfo.InvariantCulture));
        }
        protected string GetUriByQuery(string query)
        {
            var sb = new System.Text.StringBuilder(controllerUri);
            if (!string.IsNullOrEmpty(query))
            {
                bool setQuotMark;
                if (query[0] != '/')
                {
                    sb.Append('?');
                    setQuotMark = true;
                }
                else
                {
                    setQuotMark = query.Contains('?');
                }
                sb.Append(query);
                sb.Append(setQuotMark ? '&' : '?');
            }
            else
            {
                sb.Append('?');
            }
            sb.Append("key=");
            sb.Append(ApiKey);
            return sb.ToString();
        }
        string apiKey;
        public string ApiKey { get { return apiKey; } set { apiKey = value; } }
        Uri baseUri;
        public Uri BaseUri { get { return baseUri; } set { baseUri = value; } }
    }

}
