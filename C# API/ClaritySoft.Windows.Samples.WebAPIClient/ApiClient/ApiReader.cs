using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClaritySoft.Windows.Samples.WebAPIClient.ApiClient
{
    class ApiReader
    {
        readonly string controllerUri;
        readonly string filterRead;
        protected string FilterRead
        {
            get { return filterRead; }
        }
        public HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.BaseAddress = this.BaseUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
        protected string GetUriFromId(Guid id)
        {
            return GetUriByQuery("id=" + id.ToString());
        }
       
        protected static ExceptionInfo ProcessError(Task<HttpResponseMessage> saveTask)
        {
            {
                using (var rt = saveTask.Result.Content.ReadAsStringAsync())
                {
                    rt.Wait();
                    var exceptionInfo = new ExceptionInfo
                    {
                        UserFriendlyError =  rt.Result
                    };
                    var statusCodeDescription = string.Format("HTTP {0} {1}", (int)saveTask.Result.StatusCode,saveTask.Result.StatusCode.ToString());
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

        public ExceptionInfo PostFile(string relUrl, IEnumerable<KeyValuePair<string, string>> values, byte[] file, string fileKey, string fileName)
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
                    using (var task = client.PostAsync(GetUriByQuery(relUrl), content))
                    {
                        task.Wait();
                        if (task.Result.IsSuccessStatusCode)
                        {
                            return ExceptionInfo.Success;
                        }
                        return ProcessError(task);
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
        public T Load<T>()
            where T : class
        {
            return Load<T>("");
        }
        public T Load<T>(string url)
            where T : class
        {
            return LoadByUrl<T>(GetUriByQuery(url));
        }
        private string lastUrlUsed;
        public string LastUrlUsedToQuery
        {
            get { return lastUrlUsed; }
        }
        protected T LoadByUrl<T>(string url)
            where T : class
        {
            using (var client = CreateClient())
            {
                lastUrlUsed = new Uri(client.BaseAddress, url).ToString();
                System.Diagnostics.Trace.TraceInformation("Querying url: {0}", lastUrlUsed);
                using (var t = client.GetAsync(url))
                {
                    return Read<T>(t,true);
                }
            }
        }
        public void Put<T>(Guid id, T value)
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
                UpdateDatabase<T>(Put<T>(value, id, client));
            }
        }
        public void Put<T>(T value)
            where T : class,IHasId
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            var id = value.Id;
            Put(id, value);
            
        }

        public void Post<T>(T value)
            where T:class
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            using (var client = CreateClient())
            {
                UpdateDatabase<T>(Post<T>(value, client));
            }
        }
        public void Delete(Guid id)
        {
            using (var client = CreateClient())
            {
                using (var t = client.DeleteAsync(GetUriFromId(id)))
                {
                    t.Wait();
                    if (t.Result.IsSuccessStatusCode)
                    {
                        return;
                    }
                    throw new Exception(ProcessError(t).ToString());
                }


                
            }
        }
        

        protected void UpdateDatabase<T>(Task<HttpResponseMessage> task)
            where T:class
        {
            Read<T>(task,false);
        }
        protected Task<HttpResponseMessage> Post<T>(T value, HttpClient client)
        {
            return client.PostAsJsonAsync<T>(GetUriByQuery(null),value);
        }
        protected Task<HttpResponseMessage> Put<T>(T value, Guid id, HttpClient client)
        {
            return client.PutAsJsonAsync<T>(GetUriFromId(id),value);
        }
        protected static T Read<T>(System.Threading.Tasks.Task<HttpResponseMessage> t, bool performRead)
            where T : class
        {
            t.Wait();
            if (t.Result.IsSuccessStatusCode)
            {
                if (performRead)
                {
                    try
                    {
                        var formatters = new MediaTypeFormatter[] {
                    new JsonMediaTypeFormatter(),
                    new XmlMediaTypeFormatter()
                    };
                        using (var tr = t.Result.Content.ReadAsAsync<T>(formatters))
                        {
                            tr.Wait();
                            return tr.Result;
                        }
                    }
                    catch// (Exception ex)
                    {
                        throw;
                    }
                }
            }
            else
            {
                var info = ProcessError(t);
                if (info.HasValue)
                {
                    throw new Exception(info.ToString());
                }
            }

            return null;
        }
        public IEnumerable<T> QueryListByFilter<T>(string filter)
        {
            return QueryListByFilter<T>(filter, null);
        }
        public IEnumerable<T> QueryListByFilter<T>(string filter, string urlParameters)
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
            var result = QueryList<T>(useFilter);
            return result ?? new T[] { };
        }

        protected IEnumerable<T> QueryList<T>(string query)
        {
            return LoadByUrl<IEnumerable<T>>(GetUriByQuery(query)) ?? new T[] { };
        }
        public IEnumerable<T> QueryList<T>()
        {
            return QueryListByFilter<T>("");
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
