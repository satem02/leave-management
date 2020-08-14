using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Common.Models
{
    public class Result
    {
        internal Result(bool succeeded, IEnumerable<string> errors, object data)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
            Data = data;
        }

        public bool Succeeded { get; set; }
        public object Data { get; set; }

        public string[] Errors { get; set; }

        public static Result Success(object data = null)
        {
            return new Result(true, new string[] { }, data);
        }

        public static async Task<Result> SuccessAsync(object data = null)
        {
            return await Task.Run(() =>
            {
                return new Result(true, new string[] { }, data);
            });
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors, null);
        }

        public static async Task<Result> FailureAsync(IEnumerable<string> errors)
        {
            return await Task.Run(() =>
            {
                return new Result(false, errors, null);
            });
        }

        public static Result Failure(string error)
        {
            return new Result(false, new List<string>() { error }, null);
        }

        public static async Task<Result> FailureAsync(string error)
        {
            return await Task.Run(() =>
            {
                return new Result(false, new List<string>() { error }, null);
            });
        }
    }
}
