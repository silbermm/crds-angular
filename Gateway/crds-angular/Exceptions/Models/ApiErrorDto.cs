using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace crds_angular.Exceptions.Models
{
    public class ApiErrorDto
    {
        public ApiErrorDto()
        {
        }

        public ApiErrorDto(string message, Exception exception = null)
        {
            this.Message = message;

            if (exception != null)
            {
                var errors = new List<string> {exception.Message};
                if (exception.InnerException != null)
                {
                    errors.Add(exception.InnerException.Message);
                }
                this.Errors = errors;
            }
        }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "errors", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Errors { get; set; }

        [JsonIgnore]
        public HttpResponseMessage HttpResponseMessage
        {
            get
            {
                var json = JsonConvert.SerializeObject(this, Formatting.Indented);
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest) {Content = new StringContent(json)};
                return resp;
            }
        }
    }
}