namespace Business.Utilities.Result
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public T? Data { get; }
        public DataResult(T data, bool success, string message) : base(success, message)
        {
            Data = data;
        }
        public DataResult(T data, bool succes) : base(succes)
        {
            Data = data;
        }


    }
    public class Result : IResult
    {
        public Result(bool success, string message) : this(success)
        {
            Message = message;
        }
        public Result(bool success) => Success = success;

        public bool Success { get; }

        public string? Message { get; }
    }
}
