namespace ReadingCommunityApi.Core.Dtos;
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