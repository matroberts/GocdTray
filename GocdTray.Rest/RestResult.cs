using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GocdTray.Rest
{
    public class RestResult<T>
    {
        public T Data { get; set; }
        public bool HasData { get; set; }
        public RestError Error { get; set; }

        public static RestResult<T> Valid(T data) => new RestResult<T> { HasData = true, Data = data };
        public static RestResult<T> Invalid(string errorMessage, int statusCode) => new RestResult<T> {HasData = false, Data = default(T), Error = new RestError(statusCode, errorMessage)};

        public override string ToString() => HasData ? Data.ToString() : Error.ToString();
    }

    //$"Response status code does not indicate success: ({(int)response.StatusCode}) {response.StatusCode.ToString()}"

    public class RestError
    {
        public RestError(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; } = 200;
        public override string ToString() => Message;
    }
}
