using VSRESTClient.UI.Utils;

namespace VSRESTClient.UI.Models
{
    public class SearchbarModel
    {
        public string RequestUrl { get; set; }
        public SupportedHttpMethods HttpMethod { get; set; } = SupportedHttpMethods.GET;
        public string CurrentHttpMethod { get => HttpMethod.ToString(); }


        public void SetCurrentHttpMethod(SupportedHttpMethods method) => HttpMethod = method;
    }
}
