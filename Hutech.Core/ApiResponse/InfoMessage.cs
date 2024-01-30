using System.Net;

namespace Hutech.Core.ApiResponse
{
    public class InfoMessage
    {
        public InfoMessage()
            : this(string.Empty, string.Empty)
        {
        }

        public InfoMessage(string message)
            : this("", message)
        {
        }

        public InfoMessage(string key, string message)
        {
            Title = key;
            MessageText = message;
        }

        public string Title { get; set; }

        public string MessageText { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public override string ToString()
        {
            return string.Format("{0}. Key: '{1}', Message: '{2}'", base.ToString(), Title, MessageText);
        }
    }
}
