using System.Diagnostics.Contracts;

namespace Common.Results;

public static class CombinationExtensions
{
    [Pure] public static Result<(T1 FirstResult, T2 SecondResult)> CombineWith<T1, T2>(this Result<T1> first, Result<T2> second)
    {
        return Result.Combine(first, second);
    }
    [Pure] public static Result<TValue> CombineWith<TValue>(this Result<TValue> first, Result second)
    {
        return Result.Collapse(first, second);
    }
    [Pure]public static Result<TValue> CombineWith<TValue>(this Result first, Result<TValue>  second)
    {
        return Result.Collapse(second, first);
    }
    
    public static Result<TTarget> CollapseBy<T1,T2, TTarget>(this Result<(T1 FirstResult, T2 SecondResult)> result
        , Func<T1, T2, TTarget> collapse)
    {
        return result.IsFailure ? Result.Failure<TTarget>(result.Errors) : Result.Success(collapse(result.Value.FirstResult,result.Value.SecondResult));
    }

    

    
}