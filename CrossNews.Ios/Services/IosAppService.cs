using CrossNews.Core.Services;
using Foundation;

namespace CrossNews.Ios.Services
{
    public class IosAppService : IAppService
    {
        public IosAppService()
        {
            var plist = NSBundle.MainBundle.InfoDictionary;

            Version = plist["CFBundleShortVersionString"].ToString();
            BuildNumber = int.Parse(plist["CFBundleVersion"].ToString());
            Name = plist["CFBundleName"].ToString();
            Platform = "iOS";
        }

        public string Version { get; }

        public int BuildNumber { get; }

        public string Name { get; }

        public string Platform { get; }
    }
}
