namespace ProvisionPadel.Api.Shared.Results;

public class Result<T>
{
    public T? Value { get; set; }
    public Error? Error { get; set; }
    public List<Error>? Errors { get; set; }
    public bool IsSuccess => Error == null && (Errors == null || !Errors.Any());

    public Result(T value)
    {
        Value = value;
        Error = null;
        Errors = null;
    }

    public Result(Error error)
    {
        Error = error;
        Errors = null;
        Value = default;
    }

    public Result(List<Error> errors)
    {
        Errors = errors;
        Error = null;
        Value = default;
    }

    public static Result<T> Success(T value) => new Result<T>(value);
    public static Result<T> Failure(Error error) => new Result<T>(error);
    public static Result<T> Failure(List<Error> errors) => new Result<T>(errors);

    public TResult Map<TResult>(
    Func<T, TResult> onSuccess,
    Func<Error, TResult> onSingleFailure,
    Func<List<Error>, TResult> onMultipleFailures)
    {
        if (IsSuccess)
            return onSuccess(Value!);

        if (Errors != null && Errors.Any())
            return onMultipleFailures(Errors);

        return onSingleFailure(Error!);
    }

}