namespace TESTPROJESI.Core.Common
{
    /// <summary>
    /// 🎯 Result Pattern - İşlem sonuçlarını standart şekilde döndürür
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; protected set; }
        public bool IsFailure => !IsSuccess;
        public string? ErrorMessage { get; protected set; }
        public List<string> Errors { get; protected set; } = new();

        protected Result(bool isSuccess, string? errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;

            if (!string.IsNullOrEmpty(errorMessage))
                Errors.Add(errorMessage);
        }

        public static Result Success() => new(true);
        public static Result Failure(string errorMessage) => new(false, errorMessage);

        public static Result Failure(List<string> errors)
        {
            var result = new Result(false);
            result.Errors = errors;
            result.ErrorMessage = string.Join(", ", errors);
            return result;
        }
    }

    public class Result<T> : Result
    {
        public T? Data { get; private set; }

        private Result(bool isSuccess, T? data, string? errorMessage = null)
            : base(isSuccess, errorMessage)
        {
            Data = data;
        }

        public static Result<T> Success(T data) => new(true, data);
        public new static Result<T> Failure(string errorMessage) => new(false, default, errorMessage);

        public new static Result<T> Failure(List<string> errors)
        {
            var result = new Result<T>(false, default);
            result.Errors = errors;
            result.ErrorMessage = string.Join(", ", errors);
            return result;
        }

        public Result<TNew> Map<TNew>(Func<T, TNew> mapper)
        {
            if (IsFailure)
                return Result<TNew>.Failure(ErrorMessage ?? "Unknown error");

            return Result<TNew>.Success(mapper(Data!));
        }

        public Result<T> OnSuccess(Action<T> action)
        {
            if (IsSuccess && Data != null)
                action(Data);

            return this;
        }

        public Result<T> OnFailure(Action<string> action)
        {
            if (IsFailure && ErrorMessage != null)
                action(ErrorMessage);

            return this;
        }
    }
}
