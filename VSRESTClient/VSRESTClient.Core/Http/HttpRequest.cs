using System.Collections.Generic;
using VSRESTClient.Core.Utils;

namespace VSRESTClient.Core.Http
{
    public class HttpRequest
    {
        public HttpRequest() { }

        public HttpRequest(string baseUrl, ICollection<HttpParam> @params, ICollection<HttpHeader> headers, SupportedHttpMethods httpMethod, object requestBody = null, object requestBodyType = null)
        {
            BaseUrl = baseUrl;
            Params = @params;
            Headers = headers;
            HttpMethod = httpMethod;

            if (requestBody != null)
                RequestBody = requestBody;

            if (requestBodyType != null)
                RequestBodyType = requestBodyType;
        }

        public string BaseUrl { get; set; }
        public ICollection<HttpParam> Params { get; set; }
        public ICollection<HttpHeader> Headers { get; set; }
        public SupportedHttpMethods HttpMethod { get; set; }
        public object RequestBody { get; set; }
        public object RequestBodyType { get; set; }
    }
}
