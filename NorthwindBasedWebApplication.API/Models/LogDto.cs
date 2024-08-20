using NorthwindBasedWebApplication.API.Models.Base;

namespace NorthwindBasedWebApplication.API.Models
{
    public class LogDto : BaseEntity
    {
        public string Message { get; set; }
        public string Level { get; set; }
        public string Timestamp { get; set; }
        public string MethodType { get; set; }
        public string User { get; set; }
        public string Role { get; set; }
        public string Details { get; set; }
        public string StatusCode { get; set; }
        public bool Success { get; set; }
        public bool Failed { get; set; }
        public string? ErrorMessage { get; set; }
       
    }
}
