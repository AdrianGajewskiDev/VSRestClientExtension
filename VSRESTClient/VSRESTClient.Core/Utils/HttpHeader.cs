namespace VSRESTClient.Core.Utils
{
    public class HttpHeader
    {
        public HttpHeader() { }
        public HttpHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
