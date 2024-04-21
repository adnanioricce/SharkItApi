using System.ComponentModel.DataAnnotations;

public record struct Error(string Message);
public readonly record struct Result<T>(T Value,params Error[] Errors);
public static class Failure{
    public static Error ToError(this ValidationResult r) => new Error(r.ErrorMessage);
}
public static class Result
{
    public static Result<T> Ok<T>(T value) => new(value);
    public static Result<T> Fail<T>(Error error) => new(default,error);
    public static Result<T> Fail<T>(IEnumerable<Error> errors) => new(default,errors.ToArray());
    public static bool IsSuccess<T>(this Result<T> result) => !(result.Errors ?? Enumerable.Empty<Error>()).Any();    
}
// 
// public record struct Failure {
//     public readonly Error Message;
//     public Failure(string message) : this(new Error(message ?? ""))
//     {        
//     }
//     public Failure(Error error){
//         Message = error;
//     }
// }

// public struct Result {
//     // This is != null iff this is a success.
//     private readonly string _message;
//     private Result(string msg) {
//         _message = msg;
//     }
//     private Result(Error msg) {
//         _message = msg;
//     }
//     public bool IsSuccess { get { return _message == null; } }
//     public string Message {
//         get {
//             if (IsSuccess)
//                 throw new System.InvalidOperationException("no text available");
//             return _message;
//         }
//     }

//     public Failure Failure {
//         get {
//             if (IsSuccess)
//                 throw new System.InvalidOperationException("not a failure");
//             return new Failure(_message);
//         } 
//     }

//     public static Result MakeFailure(string msg) {
//         if (msg == null)
//             msg = "";
//         return new Result(msg);
//     }
//     public static Result MakeFailure(Error msg) {
//         return new Result(msg);
//     }
//     public static Result Success() {
//         return new Result(null);
//     }

//     public static Result<T> Success<T>(T value) {
//         return Result<T>.Success(value);
//     }

//     public static Failure Fail(string msg) {
//         return new Failure(msg);
//     }

//     public static implicit operator Result(Failure f) {
//         return Result.MakeFailure(f.Message);
//     }
// }

// public struct Result<T> {
//     private readonly T _data;
//     private readonly string _message;
//     private Result(T data, string msg) {
//         _message = msg;
//         _data = data;
//     }

//     public bool IsSuccess { get { return _message == null; } }
//     public string Message {
//         get {
//             if (IsSuccess)
//                 throw new System.InvalidOperationException("no text available");
//             return _message;
//         }
//     }

//     public T Data {
//         get {
//             if (!IsSuccess)
//                 throw new System.InvalidOperationException("not a success");
//             return _data;
//         }
//     }

//     public Failure Failure {
//         get {
//             if (IsSuccess)
//                 throw new System.InvalidOperationException("not a failure");
//             return new Failure(_message);
//         } 
//     }

//     public Result AsResult() {
//         if (IsSuccess)
//             return Result.Success();
//         return Failure;
//     }

//     public static Result<T> MakeFailure(string msg) {
//         if (msg == null)
//             msg = "";
//         return new Result<T>(default(T), msg);
//     }

//     public static Result<T> Success(T value) {
//         return new Result<T>(value, null);
//     }

//     public static implicit operator Result<T>(Failure f) {
//         return Result<T>.MakeFailure(f.Message);
//     }
// }

// public static class ResultExtensions {
//     public static void Match(this Result r, Action onSuccess, Action<string> onFailure) {
//         if (r.IsSuccess)
//             onSuccess();
//         else
//             onFailure(r.Message);
//     }

//     public static T Match<T>(this Result r, Func<T> onSuccess, Func<string, T> onFailure) {
//         if (r.IsSuccess)
//             return onSuccess();
//         else
//             return onFailure(r.Message);
//     }

//     public static Result Bind(this Result r, Func<Result> next) {
//         if (r.IsSuccess)
//             return next();
//         return r;
//     }

//     public static Result<T> Bind<T>(this Result r, Func<Result<T>> next) {
//         if (r.IsSuccess)
//             return r.next();
//         return Result<T>.MakeFailure(r.Message);
//     }

//     public static Result<T> Bind<T>(this Result r, T next) {
//         if (r.IsSuccess)
//             return Result<T>.Success(next);
//         return Result<T>.MakeFailure(r.Message);    
//     }

//     public static Result<T> Bind<T>(this Result r, Func<T> next) {
//         if (r.IsSuccess)
//             return Result<T>.Success(next());
//         return Result<T>.MakeFailure(r.Message);
//     }

//     public void Match<T>(this Result<T> r, Action<T> onSuccess, Action<string> onFailure) {
//         if (r.IsSuccess)
//             onSuccess(r.Data);
//         else
//             onFailure(r.Message);
//     }

//     public TResult Match<T, TResult>(this Result<T> r, Func<T, TResult> onSuccess, Func<string, TResult> onFailure) {
//         if (r.IsSuccess)
//             return onSuccess(r.Data);
//         else
//             return onFailure(r.Message);
//     }

//     public Result<R> Map<T, R>(this Result<T> r, Func<T, R> f) {
//         if (r.IsSuccess)
//             return Result<R>.Success(f(r.Data));
//         return Result<R>.MakeFailure(r.Message);
//     }

//     public Result<TResult> Bind<T, TResult>(this Result<T> r, Func<T, Result<TResult>> next) {
//         if (r.IsSuccess)
//             return next(r.Data);
//         return Result<TResult>.MakeFailure(r.Message);
//     }

//     public Result<T> Bind(this Result<T> r, Func<T, Result> next) {
//         if (r.IsSuccess)
//             return next(r.Data).Bind(r.Data);
//         return r;
//     }
// }