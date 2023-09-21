using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;
using Syncfusion.Blazor.PdfViewer;
using Syncfusion.Blazor.Kanban.Internal;
using System.Reflection.PortableExecutable;

namespace WeeklyReview.Client.Http
{
    public class WeeklyReviewApiClient
    {
        private string _baseUrl = "";
        private HttpClient _httpClient;
        private Lazy<JsonSerializerOptions> _settings;

        public bool ReadResponseAsString { get; set; }

        public WeeklyReviewApiClient(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("WeeklyReview.ServerAPI");
            BaseUrl = _httpClient.BaseAddress.ToString();
            _settings = new Lazy<JsonSerializerOptions>(CreateSerializerSettings);
        }

        private JsonSerializerOptions CreateSerializerSettings()
        {
            var settings = new JsonSerializerOptions();
            UpdateJsonSerializerSettings(settings);
            return settings;
        }

        public string BaseUrl
        {
            get { return _baseUrl; }
            set { _baseUrl = value; }
        }
        protected JsonSerializerOptions JsonSerializerSettings { get { return _settings.Value; } }

        #region Request Pipeline
        void UpdateJsonSerializerSettings(JsonSerializerOptions settings)
        {

        }

        void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder)
        {
        }

        void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
        }

        void ProcessResponse(HttpClient client, HttpResponseMessage response)
        {

        }
        #endregion

        public virtual async Task<T> GETAsync<T>(string endpoint, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append(endpoint);
            var client_ = _httpClient;

            var url = urlBuilder_.ToString();
            var response = await client_.GetAsync(url);
            if (response is null)
                throw new ApiException("Response was null which was not expected.", (int)response.StatusCode, response.ReasonPhrase, (IReadOnlyDictionary<string, IEnumerable<string>>)response.Headers.ToList().ConvertAll(x => new KeyValuePair<string, IEnumerable<string>>(x.Key, x.Value)), null);

            var res = await response.Content.ReadFromJsonAsync<T>();
            if (res is null)
                throw new ApiException("Response was null which was not expected.");
            return res;
        }

        public virtual async Task<ICollection<T>> GETCollectionAsync<T>(string endpoint, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append(endpoint);
            var client_ = _httpClient;

            var url = urlBuilder_.ToString();
            var response = await client_.GetAsync(url);
            if (response is null)
                throw new ApiException("Response was null which was not expected.", (int)response.StatusCode, response.ReasonPhrase, (IReadOnlyDictionary<string, IEnumerable<string>>)response.Headers.ToList().ConvertAll(x => new KeyValuePair<string, IEnumerable<string>>(x.Key, x.Value)), null);

            var res = await response.Content.ReadFromJsonAsync<ICollection<T>>();
            if (res is null)
                throw new ApiException("Response was null which was not expected.");
            return res;
        }

        public virtual async Task<T> POSTAsync<T, C>(string endpoint, C body, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append(endpoint);
            var client_ = _httpClient;

            var url = urlBuilder_.ToString();
            var response = await client_.PostAsJsonAsync(url, body);
            if (response is null)
                throw new ApiException("Response was null which was not expected.", (int)response.StatusCode, response.ReasonPhrase, (IReadOnlyDictionary<string, IEnumerable<string>>) response.Headers.ToList().ConvertAll(x => new KeyValuePair<string, IEnumerable<string>>(x.Key, x.Value)), null);

            var res = await response.Content.ReadFromJsonAsync<T>();
            if (res is null)
                throw new ApiException("Response was null which was not expected.");
            return res;
        }

        public virtual async Task<T> PUTAsync<T, C>(string endpoint, C body, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append(endpoint);
            var client_ = _httpClient;

            var url = urlBuilder_.ToString();
            var response = await client_.PutAsJsonAsync(url, body);
            if (response is null)
                throw new ApiException("Response was null which was not expected.", (int)response.StatusCode, response.ReasonPhrase, (IReadOnlyDictionary<string, IEnumerable<string>>)response.Headers.ToList().ConvertAll(x => new KeyValuePair<string, IEnumerable<string>>(x.Key, x.Value)), null);

            var res = await response.Content.ReadFromJsonAsync<T>();
            if (res is null)
                throw new ApiException("Response was null which was not expected.");
            return res;
        }

        public virtual async Task<MemoryStream> PUTFileAsync<C>(string endpoint, C body, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append(endpoint);
            var client_ = _httpClient;

            var url = urlBuilder_.ToString();
            var response = await client_.PutAsJsonAsync(url, body);
            if (response is null)
                throw new ApiException("Response was null which was not expected.", (int)response.StatusCode, response.ReasonPhrase, (IReadOnlyDictionary<string, IEnumerable<string>>)response.Headers.ToList().ConvertAll(x => new KeyValuePair<string, IEnumerable<string>>(x.Key, x.Value)), null);

            var res = await ReadFileResponseAsync<MemoryStream>(response, cancellationToken).ConfigureAwait(false);
            if (res.ms is null)
                throw new ApiException("Response was null which was not expected.");
            return res.ms;
        }

        public virtual async Task<ICollection<T>> PUTCollectionAsync<T, C>(string endpoint, C body, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append(endpoint);
            var client_ = _httpClient;

            var url = urlBuilder_.ToString();
            var response = await client_.PutAsJsonAsync(url, body);
            if (response is null)
                throw new ApiException("Response was null which was not expected.", (int)response.StatusCode, response.ReasonPhrase, (IReadOnlyDictionary<string, IEnumerable<string>>)response.Headers.ToList().ConvertAll(x => new KeyValuePair<string, IEnumerable<string>>(x.Key, x.Value)), null);

            var res = await response.Content.ReadFromJsonAsync<ICollection<T>>();
            if (res is null)
                throw new ApiException("Response was null which was not expected.");
            return res;
        }

        public virtual async Task<T> DELETEAsync<T, C>(string endpoint, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append(endpoint);
            var client_ = _httpClient;

            var url = urlBuilder_.ToString();
            var response = await client_.DeleteAsync(url);
            if (response is null)
                throw new ApiException("Response was null which was not expected.", (int)response.StatusCode, response.ReasonPhrase, (IReadOnlyDictionary<string, IEnumerable<string>>)response.Headers.ToList().ConvertAll(x => new KeyValuePair<string, IEnumerable<string>>(x.Key, x.Value)), null);

            var res = await response.Content.ReadFromJsonAsync<T>();
            if (res is null)
                throw new ApiException("Response was null which was not expected.");
            return res;
        }

        public virtual async Task DELETEAsync(string endpoint, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append(endpoint);
            var client_ = _httpClient;

            var url = urlBuilder_.ToString();
            var response = await client_.DeleteAsync(url);
            if (response is null)
                throw new ApiException("Response was null which was not expected.", (int)response.StatusCode, response.ReasonPhrase, (IReadOnlyDictionary<string, IEnumerable<string>>)response.Headers.ToList().ConvertAll(x => new KeyValuePair<string, IEnumerable<string>>(x.Key, x.Value)), null);
        }

        #region Helpers
        protected virtual async Task<ObjectResponseResult<T>> ReadFileResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            try
            {
                var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var ms = new MemoryStream();
                responseStream.CopyToAsync(ms, cancellationToken).ConfigureAwait(false);
                var typedBody = Activator.CreateInstance(typeof(T));

                return new ObjectResponseResult<T>((T)typedBody, string.Empty, ms);

            }
            catch (JsonException exception)
            {
                var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                throw new ApiException("Response was null which was not expected.", (int)response.StatusCode, response.ReasonPhrase, (IReadOnlyDictionary<string, IEnumerable<string>>)response.Headers.ToList().ConvertAll(x => new KeyValuePair<string, IEnumerable<string>>(x.Key, x.Value)), null);
            }
        }

        protected struct ObjectResponseResult<T>
        {
            public ObjectResponseResult(T responseObject, string responseText, MemoryStream memoryStream)
            {
                Object = responseObject;
                Text = responseText;
                ms = memoryStream;
            }

            public T Object { get; }

            public string Text { get; }

            public MemoryStream? ms { get; }
        }
        #endregion
    }
}
