using System;

namespace CrossNews.Core.Services
{
    public interface IReachabilityService
    {
        bool IsConnectionAvailable { get; }
    }
}
