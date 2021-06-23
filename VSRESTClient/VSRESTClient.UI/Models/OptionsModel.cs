using System;
using System.Collections.Generic;
using VSRESTClient.Core.Utils;
using VSRESTClient.UI.Utils;

namespace VSRESTClient.UI.Models
{
    public class OptionsModel
    {
        public List<HttpHeader> Headers { get; set; }
        public List<HttpParam> HttpParams { get; set; }

        public OptionsModel()
        {
            Headers = new List<HttpHeader>();
            HttpParams = new List<HttpParam>();
        }

      
    }
}
