
using Common.Errors;

namespace Common.Results;

public static class FailureExtensions
{
    public static async Task<Result<TValue>> FailIf<TValue>(this Task<Result<TValue>> resultTask, Predicate<TValue> predicate,  Error error)
    {
        var result = await resultTask;
        if (predicate(result.Value)) return Result.Failure<TValue>(error);
        return result;
    }

    
    public static Result<TValue> FailIf<TValue>(this Result<TValue> result, Predicate<Result<TValue>> predicate, Error error)
    {
        if (predicate(result)) return Result.Failure<TValue>(error);
        return result;
    }
    
    public static async Task<Result<TValue>> FailIf<TValue>(this Task<Result<TValue>> resultTask, Predicate<Result<TValue>> predicate, Error error)
    {
        var result = await resultTask;
        return result.FailIf(predicate,error);
    }
    

}