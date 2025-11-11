namespace TESTPROJESI.Core.Builders
{
    /// <summary>
    /// 🏗️ Request Builder Pattern
    /// API isteklerini fluent interface ile oluşturur
    /// </summary>
    public class ApiRequestBuilder
    {
        private string _endpoint = string.Empty;
        private readonly Dictionary<string, string> _queryParams = new();
        private readonly Dictionary<string, string> _headers = new();
        private object? _body;
        private string _method = "GET";

        public static ApiRequestBuilder Create() => new();

        public ApiRequestBuilder WithEndpoint(string endpoint)
        {
            _endpoint = endpoint;
            return this;
        }

        public ApiRequestBuilder WithMethod(string method)
        {
            _method = method;
            return this;
        }

        public ApiRequestBuilder WithQueryParam(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                _queryParams[key] = value;
            return this;
        }

        public ApiRequestBuilder WithQueryParams(Dictionary<string, string>? parameters)
        {
            if (parameters == null) return this;

            foreach (var param in parameters)
            {
                if (!string.IsNullOrWhiteSpace(param.Value))
                    _queryParams[param.Key] = param.Value;
            }
            return this;
        }

        public ApiRequestBuilder WithHeader(string key, string value)
        {
            _headers[key] = value;
            return this;
        }

        public ApiRequestBuilder WithBody(object body)
        {
            _body = body;
            return this;
        }

        public ApiRequestBuilder WithLimit(int limit)
        {
            return WithQueryParam("limit", limit.ToString());
        }

        public ApiRequestBuilder WithSort(string field, bool descending = false)
        {
            var sortValue = descending ? $"{field} DESC" : field;
            return WithQueryParam("sort", sortValue);
        }

        public ApiRequestBuilder WithFilter(string field, string value)
        {
            return WithQueryParam(field, value);
        }

        /// <summary>
        /// Full URL'i oluşturur (endpoint + query string)
        /// </summary>
        public string BuildUrl()
        {
            if (_queryParams.Count == 0)
                return _endpoint;

            var queryString = string.Join("&", _queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            return $"{_endpoint}?{queryString}";
        }

        /// <summary>
        /// Request bilgilerini döndürür
        /// </summary>
        public (string Url, string Method, object? Body, Dictionary<string, string> Headers) Build()
        {
            return (BuildUrl(), _method, _body, _headers);
        }
    }
}
