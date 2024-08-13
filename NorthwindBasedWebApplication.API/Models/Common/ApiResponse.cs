using System.Net;

namespace NorthwindBasedWebApplication.API.Models.Common
{
    public class ApiResponse
    {

        public ApiResponse()
        {
            ErrorMessages = new List<string>();
            data = null;
        }

        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object data { get; set; }
    }
}
