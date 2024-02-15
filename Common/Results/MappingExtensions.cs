using System.Diagnostics.Contracts;

namespace Common.Results;

public static class MappingExtensions
{
    [Pure] public static Result<TTarget> MapTo<TValue, TTarget>(this Result<TValue> result, Func<TValue, TTarget> map)
    {
        if (result.IsFailure) return Result.Failure<TTarget>(result.Errors);
        return Result.Success(map(result.Value));
    }
    
    [Pure] public static async Task<Result<TTarget>> MapTo<TValue, TTarget>(this Task<Result<TValue>> resultTask, Func<TValue, TTarget> map)
    {
        var result = await resultTask;
        return result.MapTo(map);
    }

    public static Result<TValue> PipeTo<TValue>(this Result<TValue> result, Func<TValue, Result> action)
    {
        if (result.IsFailure) return Result.Failure<TValue>(result.Errors);
        var actionResult = action(result.Value);
        if (actionResult.IsSuccess) return result;
        return Result.Failure<TValue>(actionResult.Errors);
    }
    
    public static async Task<Result<TValue>> PipeTo<TValue>(this Result<TValue> result, Func<TValue, Task<Result>> action)
    {
        if (result.IsFailure) return Result.Failure<TValue>(result.Errors);
        var actionResult = await action(result.Value);
        if (actionResult.IsSuccess) return result;
        return Result.Failure<TValue>(actionResult.Errors);
    } 
    
    public static async Task<Result<TValue>> PipeTo<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task<Result>> action)
    {
        Result<TValue> result = await resultTask;
        if (result.IsFailure) return Result.Failure<TValue>(result.Errors);
        
        var actionResult = await action(result.Value);
        if (actionResult.IsSuccess) return result;
        return Result.Failure<TValue>(actionResult.Errors);
    } 
    
    
    public static Result<TValue> PipeTo<TValue>(this Result<TValue> result, Action<TValue> action)
    {
        if (result.IsSuccess) action(result.Value);
        return result;
    } 
    
    public static async Task<Result<TValue>> PipeTo<TValue>(this Result<TValue> result, Func<TValue, Task<Result<TValue>>> action)
    {
        if (result.IsFailure) return Result.Failure<TValue>(result.Errors);
        var actionResult = await action(result.Value);
        if (actionResult.IsSuccess) return result;
        return Result.Failure<TValue>(actionResult.Errors);
    } 
    
    
    public static Result<TValue> Do<TValue>(this Result<TValue> result, Action<TValue> action)
    {
        if (result.IsFailure) return Result.Failure<TValue>(result.Errors);
        action(result.Value);
        return result;
    } 
    
        
    public static async Task<Result<TValue>> Do<TValue>(this Task<Result<TValue>> resultTask, Action<TValue> action)
    {
        var result = await resultTask;
        return result.Do(action);
    } 
}