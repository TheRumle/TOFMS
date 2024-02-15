using System.Diagnostics.Contracts;
using System.Runtime.InteropServices.JavaScript;
using Common.Errors;

namespace Common.Results;


public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Errors = new[] {error};
    }
    protected internal Result(bool isSuccess, Error[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }
    protected internal Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
        Errors = new Error[] { };
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error[] Errors { get; }

    public static Result Success() => new(true);
    public static Result<TValue> Success<TValue>(TValue value) => new(value);

    [Pure] public static Result Failure(Error error) => new(false, error);
    [Pure] public static Result Failure(Error[] errors) => new(false, errors);
    [Pure] public static Result<T> Failure<T>(Error[] errors) => new(default,false, errors);
    [Pure] public static Result<T> Failure<T>() => new(default,false, []);
    [Pure] public static Result<T> Failure<T>(Error errors) => new(default,false, errors);
    [Pure] public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullError);
    
    [Pure] public static Result<TValue> Create<TValue>(TValue? value, Error fallback) =>
        value is not null ? Success(value) : Failure<TValue>(fallback);
    
    [Pure] public static Result<TValue> Create<TValue>(TValue? value, Func<Error> errorFactory) =>
        value is not null ? Success(value) : Failure<TValue>(errorFactory());
    

    public static Result<T> EnsureThat<T>(T value, Predicate<T> predicate, Error error)
    {
        return predicate(value) ? Success(value) : Failure<T>(error);
    }
    
    public static Result<T> EnsureThat<T>(T value, params (Predicate<T> predicate, Error error)[] predicates)
    {
        var res = new List<Result<T>>();
        foreach (var (predicate, error) in predicates)    
            res.Add(EnsureThat(value,predicate, error));

        return Combine(res.ToArray());
    }

    [Pure] public static Result<T> Combine<T>(params Result<T>[] results)
    {
        if (results.Any(r => r.IsFailure))
        {
            return Failure<T>(
                results.SelectMany(e => e.Errors).Where(e => e != Error.None)
                    .Distinct()
                    .ToArray()
            );
        }

        return Success(results[0].Value);
    }

    [Pure] public static Result<(T1 FirstResult, T2 SecondResult)> Combine<T1, T2>(Result<T1> first, Result<T2> second)
    {
        if (first.IsFailure || second.IsFailure)
        {
            var errors = second.Errors.Where(e => e != Error.None).ToList();
            errors.AddRange(first.Errors);
            return Failure<(T1 FirstResult, T2 SecondResult)> (errors.ToArray());
        }
        return Success((first.Value, second.Value));
    }
    
    [Pure] public static Result<TValue> Collapse<TValue>(Result<TValue> first, Result second)
    {
        if (first.IsFailure || second.IsFailure)
            return Failure<TValue>(CombineErrors(first,second));
        
        return first;
    }
    
    [Pure] public static Result<TValue> CollapseInto<TValue>(Result<TValue> first, params Result[] results)
    {
        if (first.IsFailure || results.Any(IsFailed))
        {
            return Failure<TValue>(CombineErrors(new[] { first }.Concat(results)));
        }
        
        return first;
    }

    [Pure] public static Error[] CombineErrors(Result first, Result second)
    {
        var errs = new List<Error>(first.Errors.Length + second.Errors.Length);
        errs.AddRange(first.Errors);
        errs.AddRange(second.Errors);
        return errs.ToArray();
    }
    
    [Pure] public static Error[] CombineErrors(IEnumerable<Result> rest)
    {
        return rest.SelectMany(e => e.Errors).ToArray();
    }

    [Pure] public static bool IsSuccessFull(Result r)
    {
        return r.IsSuccess;
    }
    
    [Pure] public static bool IsFailed(Result r)
    {
        return r.IsFailure;
    }

    
}

public class Result<T> : Result
{
    internal Result(T value, bool isSuccess, Error error) 
        : this(value, isSuccess, new []{error})
    {
    }
    internal Result(T value) : this(value, true, new Error[]{})
    {
    }

    internal Result(T value, bool isSuccess, Error[] errors) : base(isSuccess, errors)
    {
        Value = value;
    }
    public T Value { get; private set; }
}