using VSRESTClient.Core.Utils;
using VSRESTClient.UI.Utils;

namespace VSRESTClient.UI.Models
{
    public class BodyContentModel
    {
        public string BodyContentType { get; set; }
        public string Content { get; set; }


        public BodyContentModel()
        {
            BodyContentType = BodyContentTypes.Application_Json;
            Content = "{ }";
        }
    }
}
