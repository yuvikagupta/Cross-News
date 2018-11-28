using System.Collections.Generic;
using CrossNews.Core.Model;

namespace CrossNews.Core.Services
{
    internal class DummyPlatformLicenseList : IPlatformLicenseList
    {
        public List<LicenseInfo> PlatformLicenses => null;
    }
}
