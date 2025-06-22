using Newtonsoft.Json;

namespace Raftlabs.Library.Exceptions;

public class ApiException
{
    public Guid ExceptionId { get; set; }

    public string Message { get; set; }

    public virtual string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}