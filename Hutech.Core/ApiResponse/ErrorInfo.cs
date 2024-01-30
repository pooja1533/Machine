using System.Net;

namespace Hutech.Core.ApiResponse
{
    public class ErrorInfo
    {
        public ErrorInfo()
           : this(string.Empty, string.Empty)
        {
        }

        public ErrorInfo(string errorMessage)
            : this("", errorMessage)
        {
        }

        public ErrorInfo(string value, string errorMessage)
        {
            Value = value;
            ErrorMessage = errorMessage;
        }

        public string Value { get; set; }

        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;
        public override string ToString()
        {
            return string.Format("{0}. Key: '{1}', ErrorMessage: '{2}'", base.ToString(), Value, ErrorMessage);
        }
    }
}
