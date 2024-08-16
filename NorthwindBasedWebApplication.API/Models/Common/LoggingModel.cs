namespace NorthwindBasedWebApplication.API.Models.Common
{
    public class LoggingModel
    {
        public string Exception { get; set; }
        public string MethodType { get; set; }
        public string User { get; set; }
        public string Role { get; set; }
        public string Details { get; set; }
        public string Data { get; set; }
        public bool Success { get; set; }
        public bool Failed { get; set; }
        public string ErrorMessage { get; set; }
        public string StatusCode { get; set; }
    }
}
