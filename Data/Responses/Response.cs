namespace Geopixel.Web.Data.Responses;

public class Response<T>(string message, T data, bool isSuccess)
{
    public T Data { get; set; } = data;
    public string Message { get; set; } = message;
    public bool IsSuccess { get; set; } = isSuccess;
}