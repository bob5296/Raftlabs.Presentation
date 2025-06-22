using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raftlabs.Library.Caching.Models;

public class CacheOptions
{
    public string CacheType { get; set; } = "Memory";
    public int AbsoluteExpirationInMinutes { get; set; } = 60;
    public int SlidingExpirationInMinutes { get; set; } = 5;
    public int MemoryCacheExpirationScanFrequencyInMinutes { get; set; } = 10;
}
