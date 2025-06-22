namespace Raftlabs.Library.Models;

public class AppResponse
{
    public AppResponse(bool isSuccess = true)
    {
        IsSuccess = isSuccess;
    }

    public AppResponse(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public bool IsSuccess { get; set; }

    public string Message { get; set; }

    public string ToJson()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}