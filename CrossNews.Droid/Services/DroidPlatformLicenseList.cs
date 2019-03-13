using System.Collections.Generic;
using CrossNews.Core.Model;
using CrossNews.Core.Services;

namespace CrossNews.Droid.Services
{
    public class DroidPlatformLicenseList : IPlatformLicenseList
    {
        public List<LicenseInfo> PlatformLicenses { get; } = new List<LicenseInfo>
        {
            new LicenseInfo("Android Support Library", "Apache 2.0", "https://android.googlesource.com/platform/frameworks/support/+/refs/heads/master/LICENSE.txt"),
            new LicenseInfo("Xamarin Android Support Bindings", "MIT", "https://github.com/xamarin/AndroidSupportComponents/blob/master/LICENSE.md"),
            //new LicenseInfo("SimpleSectionedListAdapter", "None", "https://gist.github.com/gabrielemariotti/4c189fb1124df4556058")
        };
    }
}
