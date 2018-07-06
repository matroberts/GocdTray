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

        public static implicit operator RestResult<T>(T data) => new RestResult<T> { HasData = true, Data = data };
        public static implicit operator RestResult<T>(RestError error) => new RestResult<T> { HasData = false, Data = default(T), Error = error };
    }

    public class RestError
    {
        public RestError(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; } = 200;
    }
}
