using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class Result<T> : Result
    {
        public T? Data { get; }

        private Result(bool success, T? data, string message)
            : base(success, message)
        {
            Data = data;
        }

        public static Result<T> Succeeded(T data, string message = "")
        {
            return new Result<T>(true, data, message);
        }

        public static new Result<T> Failed(string message)
        {
            return new Result<T>(false, default, message);
        }
    }
}
