using Android.App;
using CrossNews.Core.Services;

namespace CrossNews.Droid.Services
{
    public class DroidAppService : IAppService
    {
        public DroidAppService()
        {
            var packageInfo = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0);
            Version = packageInfo.VersionName;
            BuildNumber = packageInfo.VersionCode;
            Name = packageInfo.ApplicationInfo.PackageName;
            Platform = "Android";
        }

        public string Version { get; }

        public int BuildNumber { get; }

        public string Name { get; }

        public string Platform { get; }
    }
}
