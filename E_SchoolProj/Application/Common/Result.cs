using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class Result
    {
        public bool Success { get; }

        public string Message { get; }

        protected Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static Result Succeeded(string message = "")
        {
            return new Result(true, message);
        }

        public static Result Failed(string message)
        {
            return new Result(false, message);
        }
    }
}
