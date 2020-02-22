using System.Net;
using Newtonsoft.Json;

namespace eTaskAdvisor.WebApi.Data.Error
{
    public class ApiError
    {
        public int StatusCode { get; private set; }

        public string StatusDescription { get; private set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; private set; }

        public ApiError(int statusCode, string statusDescription)
        {
            this.StatusCode = statusCode;
            this.StatusDescription = statusDescription;
        }

        public ApiError(int statusCode, string statusDescription, string message) : this(statusCode, statusDescription)
        {
            this.Message = message;
        }
    }

    public class ApiNotFoundError : ApiError
    {
        public ApiNotFoundError() : base(404, HttpStatusCode.NotFound.ToString())
        {
        }

        public ApiNotFoundError(string message) : base(404, HttpStatusCode.NotFound.ToString(), message)
        {
        }
    }

    public class ApiInternalServerError : ApiError
    {
        public ApiInternalServerError() : base(500, HttpStatusCode.InternalServerError.ToString())
        {
        }


        public ApiInternalServerError(string message) : base(500, HttpStatusCode.InternalServerError.ToString(),
            message)
        {
        }
    }
}