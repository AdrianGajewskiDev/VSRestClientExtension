using System.Text;
using VSRESTClient.Core.Utils;

namespace VSRESTClient.UI.Builders
{
    public class UrlBuilder
    {
        public UrlBuilder(string baseUrl)
        {
            _baseUrl = baseUrl;

            _source.Append(_baseUrl);
        }

        private StringBuilder _source = new StringBuilder();

        private string _baseUrl;

        public UrlBuilder AddQueryParam(HttpParam param)
        {
            if (!this._baseUrl.Contains("?"))
            {
                _source.Append("?");
            }
            _source.Append($"{param.Name}={param.Value}&");

            return this;
        }


        public string Build()
        {
            var url = _source.ToString();

            if (url.EndsWith("&"))
                url = url.Remove(url.Length - 1);

            return url;
        }
    }
}
