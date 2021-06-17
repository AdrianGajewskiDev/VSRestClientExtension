namespace VSRESTClient.Core.Utils
{
    public class HttpParam
    {
        public HttpParam() { }

        public HttpParam(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
