using System.Collections.Generic;
using System.Net;
using VSRESTClient.Core.Utils;

namespace VSRESTClient.Core.Http
{
    public class HttpResponse
    {
        public string Content { get; set; }
        public string ContentType { get; set; }
        public ICollection<HttpHeader> Headers { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
