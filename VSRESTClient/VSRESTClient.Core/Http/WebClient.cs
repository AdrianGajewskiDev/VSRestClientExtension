using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VSRESTClient.Core.Utils;

namespace VSRESTClient.Core.Http
{
    public class WebClient
    {
        private HttpClient _httpClient;

        public async Task<HttpResponse> SendRequestAsync(HttpRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using (_httpClient = new HttpClient())
                {
                    if (request.Headers.Any())
                    {
                        foreach (var header in request.Headers)
                        {
                            _httpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
                        }
                    }


                    HttpResponseMessage responseMessage;

                    HttpContent requestBody = null;

                    if(request.RequestBody != null)
                    {
                        requestBody = new StringContent(request.RequestBody.ToString(), Encoding.UTF8, request.RequestBodyType.ToString());
                    }

                    switch (request.HttpMethod)
                    {
                        case SupportedHttpMethods.GET:
                            responseMessage = await _httpClient.GetAsync(request.BaseUrl, cancellationToken);
                            break;
                        case SupportedHttpMethods.POST:
                            responseMessage = await _httpClient.PostAsync(request.BaseUrl, requestBody, cancellationToken);
                            break;
                        case SupportedHttpMethods.PUT:
                            responseMessage = await _httpClient.PutAsync(request.BaseUrl, requestBody, cancellationToken);
                            break;
                        case SupportedHttpMethods.DELETE:
                            responseMessage = await _httpClient.DeleteAsync(request.BaseUrl, cancellationToken);
                            break;
                        default:
                            responseMessage = await _httpClient.GetAsync(request.BaseUrl, cancellationToken);
                            break;
                    }

                    var content = await responseMessage.Content.ReadAsStringAsync();

                    return new HttpResponse
                    {
                        Content = content,
                        ContentType = responseMessage.Content.Headers.ContentType?.MediaType,
                        Headers = responseMessage.Headers.Select(x => new HttpHeader(x.Key, string.Concat(x.Value))).ToList(),
                        StatusCode = responseMessage.StatusCode
                    };
                }
            }
            catch(Exception ex)
            {
                return new HttpResponse
                {
                    Content = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

        }
    }
}
