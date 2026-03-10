namespace ReadingCommunityApi.Application.Dtos;

// Non-generic version for operations without data
public class OperationResult
{
    public bool IsSuccess { get; set; }

    public int StatusCode { get; set; }

    public string? Message { get; set; }


    public static OperationResult Success(string? message = null, int statusCode = 200) => new()
    {
        IsSuccess = true,
        StatusCode = statusCode,
        Message = message
    };

    public static OperationResult Failure(string message, int statusCode = 400) => new()
    {
        IsSuccess = false,
        StatusCode = statusCode,
        Message = message
    };
}

// Generic version for operations that return data
public class OperationResult<T>
{
    public T? Data { get; set; }

    public bool IsSuccess { get; set; }

    public int StatusCode { get; set; }

    public string? Message { get; set; }


    public static OperationResult<T> Success(T data, string? message = null, int statusCode = 201) => new()
    {
        Data = data,
        IsSuccess = true,
        StatusCode = statusCode,
        Message = message
    };

    public static OperationResult<T> Failure(string message, int statusCode = 400) => new()
    {
        IsSuccess = false,
        StatusCode = statusCode,
        Message = message
    };
}