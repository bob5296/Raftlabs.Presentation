using System.Net;
using StackExchange.Redis;

namespace Raftlabs.Library.Caching.Models;

public class RedisConfiguration
{
    private ConfigurationOptions _configurationOptions;

    public ConfigurationOptions ConfigurationOptions
    {
        get
        {
            if (EndPoints != null && !_configurationOptions.EndPoints.Any())
            {
                foreach (var endPoint in EndPoints)
                {
                    _configurationOptions.EndPoints.Add(IPAddress.Parse(endPoint.Host), endPoint.Port);
                }
            }

            return _configurationOptions;
        }

        set => _configurationOptions = value;
    }

    public string InstanceName { get; set; }

    public List<RedisEndpoint> EndPoints { get; set; }
}