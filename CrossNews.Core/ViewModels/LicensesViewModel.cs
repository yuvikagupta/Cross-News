using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CrossNews.Core.Model;
using CrossNews.Core.Services;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace CrossNews.Core.ViewModels
{
    public class LicensesViewModel : MvxViewModel
    {
        private readonly IBrowserService _browser;

        public LicensesViewModel(IBrowserService browser, IPlatformLicenseList platformLicense)
        {
            _browser = browser;

            ShowLicense = new MvxAsyncCommand<LicenseInfo>(OnShowLicense);

            CoreLicenses = new List<LicenseInfo>
            {
                new LicenseInfo("MvvmCross", "MS-PL", "https://github.com/MvvmCross/MvvmCross/blob/master/LICENSE"),
                new LicenseInfo("Json.NET", "MIT", "https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md") 
            };

            PlatformLicenses = platformLicense.PlatformLicenses;
        }

        private Task OnShowLicense(LicenseInfo obj)
        {
            return _browser.ShowInBrowserAsync(obj.LicenseUri, true);
        }

        public List<LicenseInfo> CoreLicenses { get; }
        public List<LicenseInfo> PlatformLicenses { get; }

        public ICommand ShowLicense { get; }
    }
}
