using System.Collections.Generic;
using CrossNews.Core.Model;

namespace CrossNews.Core.Services
{
    public interface IPlatformLicenseList
    {
        List<LicenseInfo> PlatformLicenses { get; }
    }
}
