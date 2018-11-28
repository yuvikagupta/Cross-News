using System.Collections.Generic;
using CrossNews.Core.Model;
using CrossNews.Core.Services;

namespace CrossNews.Ios.Services
{
    public class IosPlatformLicenseList : IPlatformLicenseList
    {
        public List<LicenseInfo> PlatformLicenses { get; } = new List<LicenseInfo>
        {
            new LicenseInfo("Xamarin.iOS", "MIT", "https://github.com/xamarin/xamarin-macios/blob/master/LICENSE")
        };
    }
}
