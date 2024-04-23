using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

public record struct Error(string Message);
public readonly record struct Result<T>(T Value, params Error[] Errors);

public static class Failure{
    public static Error ToError(this ValidationResult r) => new Error($"{r.ErrorMessage} \n {string.Join("\n", r.MemberNames)}");
}
public static class Result
{
    public static Result<T> Ok<T>(T value) => new(value);
    public static Result<T> Fail<T>(Error error) => new(default,error);
    public static Result<T> Fail<T>(IEnumerable<Error> errors) => new(default,errors.ToArray());
    public static bool IsSuccess<T>(this Result<T> result) => !(result.Errors ?? Enumerable.Empty<Error>()).Any();    
    public static Result<TOut> Map<TIn,TOut>(this Result<TIn> result, Func<TIn, TOut> mapFunc)
    {
        if (!result.IsSuccess())
            return Result.Fail<TOut>(result.Errors);
        var outValue = mapFunc(result.Value);
        return Result.Ok(outValue);
    }
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> mapFunc)
    {
        if (!result.IsSuccess())
            return Result.Fail<TOut>(result.Errors);
        return mapFunc(result.Value);
    }
    public static async Task<Result<T>> Map<T>(this Result<Task<T>> r)
    {
        if (!r.IsSuccess())
            return Result.Fail<T>(r.Errors);
        return Result.Ok(await r.Value);
    }
    public static T Unwrap<T>(this Result<T> r)
    {
        if (!r.IsSuccess())
            throw new InvalidOperationException();
        return r.Value;
    }
    public static async Task<T> UnwrapAsync<T>(this Result<Task<T>> r)
    {
        if (!r.IsSuccess())
            throw new InvalidOperationException();
        return await r.Value;
    }
}