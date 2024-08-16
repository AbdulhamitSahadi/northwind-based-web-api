namespace NorthwindBasedWebApplication.API.Models.Common
{
    public class LoggingModelBuilder
    {
        private readonly LoggingModel _logModel;

        public LoggingModelBuilder SetException(string exception)
        {
            _logModel.Exception = exception;
            return this;
        }


        public LoggingModelBuilder SetMethodType(string methodType)
        {
            _logModel.MethodType = methodType;
            return this;
        }


        public LoggingModelBuilder SetUser(string user)
        {
            _logModel.User = user;
            return this;
        }


        public LoggingModelBuilder SetRole(string role)
        {
            _logModel.Role = role;
            return this;
        }

        public LoggingModelBuilder SetDetails(string details)
        {
            _logModel.Details = details;
            return this;
        }


        public LoggingModelBuilder SetData(string data)
        {
            _logModel.Data = data;
            return this;
        }


        public LoggingModelBuilder SetSuccess()
        {
            _logModel.Success = true;
            return this;     
        }


        public LoggingModelBuilder SetFailed()
        {
            _logModel.Failed = false;
            return this;
        }                   


        public LoggingModelBuilder SetErrorMessage(string errorMessage)
        {
            _logModel.ErrorMessage = errorMessage;
            return this;    
        }


        public LoggingModelBuilder SetStatusCode(string statusCode)
        {
            _logModel.StatusCode = statusCode;
            return this;
        }

        public LoggingModel Build()
        {
            return _logModel;
        }
    }
}
