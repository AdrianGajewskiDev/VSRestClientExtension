using System;
using VSRESTClient.Core.Http;
using VSRESTClient.UI.Utils;

namespace VSRESTClient.UI.Models
{
    public class SearchbarModel
    {
        public string RequestUrl { get; set; }
        public string CurrentHttpMethod { get => HttpMethod.ToString(); }
        public SupportedHttpMethods HttpMethod { get; set; }
        public OptionsPage CurrentOptionsTab { get; set; }

        public SearchbarModel()
        {
            HttpMethod = SupportedHttpMethods.GET;
            CurrentOptionsTab = OptionsPage.Params;
            RequestUrl = StaticStrings.DefaultUrl;
        }

        public void SetCurrentHttpMethod(SupportedHttpMethods method) => HttpMethod = method;
        public void SetCurrentOptionsTabOpened(OptionsPage page) => CurrentOptionsTab = page;
        public void SetUrl(string value) => RequestUrl = value;
    }
}
