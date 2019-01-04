using System;

namespace CrossNews.Core.Model
{
    public struct LicenseInfo
    {
        public LicenseInfo(string name, string license, string url) {
            Name = name;
            LicenseName = license;
            LicenseUri = new Uri(url);
        }

        public string Name { get; }
        public string LicenseName { get; }
        internal Uri LicenseUri { get; }
    }
}
