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
        public string ErrorMessage { get; set; }
        public override string ToString() => HasData ? Data.ToString() : ErrorMessage;

        public static RestResult<T> Valid(T data) => new RestResult<T> { HasData = true, Data = data, ErrorMessage = string.Empty };
        public static RestResult<T> Invalid(string errorMessage) => new RestResult<T> { HasData = false, Data = default(T), ErrorMessage = errorMessage };
    }
}
