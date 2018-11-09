using CrossNews.Core.Services;

namespace CrossNews.Ios.Services
{
    public class IosReachabilityService : IReachabilityService
    {
        public bool IsConnectionAvailable => true;
    }
}
