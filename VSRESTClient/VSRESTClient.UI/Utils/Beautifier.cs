
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace VSRESTClient.UI.Utils
{
    public sealed class Beautifier
    {
        private static Beautifier m_Instance;


        private static object m_LockObject = new object();

        public static Beautifier Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    lock (m_LockObject)
                    {
                        m_Instance = new Beautifier();
                    }
                }

                return m_Instance;
            }
        }

        private Beautifier() { }

        public string Format(string content, string contentType)
        {
            switch (contentType)
            {
                case "application/json":
                    {
                        JToken parsedJson = JToken.Parse(content);

                        var beautified = parsedJson.ToString(Formatting.Indented);

                        return beautified;
                    }
                case "text/html":
                    {
                        var parser = new HtmlParser();
                        var parsedDocument = parser.ParseDocument(content);

                        var stringWriter = new StringWriter();

                        parsedDocument.ToHtml(stringWriter, new PrettyMarkupFormatter());
                        var beautified = stringWriter.ToString();

                        return beautified;
                    }
            }

            return content;
        }
    }
}
