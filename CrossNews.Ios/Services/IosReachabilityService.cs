using System.Net;
using CrossNews.Core.Services;
using SystemConfiguration;

namespace CrossNews.Ios.Services
{
    public class IosReachabilityService : IReachabilityService
    {
        public bool IsConnectionAvailable
        {
            get
            {
                var reach = new NetworkReachability(IPAddress.Any);

                return reach.TryGetFlags(out var flags) 
                    && flags.HasFlag(NetworkReachabilityFlags.Reachable);
            }
        }
    }
}
