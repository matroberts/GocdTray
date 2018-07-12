using System;
using System.Collections.Generic;
using System.Linq;

namespace GocdTray.App.Abstractions
{
    public class Result<T>
    {
        public T Data { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public override string ToString() => IsValid ? Data.ToString() : ErrorMessage;

        public static Result<T> Valid(T data) => new Result<T> { IsValid = true, Data = data, ErrorMessage = string.Empty };
        public static Result<T> Invalid(string errorMessage) => new Result<T> { IsValid = false, Data = default(T), ErrorMessage = errorMessage };
    }
}
