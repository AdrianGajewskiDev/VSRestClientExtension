using System;
using VSRESTClient.UI.Utils;

namespace VSRESTClient.UI.Models
{
    public class SearchbarModel
    {
        public string RequestUrl { get; set; }
        public SupportedHttpMethods HttpMethod { get; set; }
        public string CurrentHttpMethod { get => HttpMethod.ToString(); }

        public SearchbarModel()
        {
            HttpMethod = SupportedHttpMethods.GET;
            RequestUrl = StaticStrings.DefaultUrl;
        }

        public void SetCurrentHttpMethod(SupportedHttpMethods method) => HttpMethod = method;
        public void SetUrl(string value) => RequestUrl = value;
    }
}
