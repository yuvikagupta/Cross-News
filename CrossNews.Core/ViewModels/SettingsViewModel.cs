using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CrossNews.Core.Services;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CrossNews.Core.ViewModels
{
    public class SettingsViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigation;
        private readonly IBrowserService _browser;

        public SettingsViewModel(IMvxNavigationService navigation, IBrowserService browser, IAppService app)
        {
            _navigation = navigation;
            _browser = browser;

            ShowProjectSiteCommand = new MvxAsyncCommand(OnShowProjectSite);
            ShowLicensesCommand = new MvxAsyncCommand(OnShowLicenses);

            VersionString = $"{app.Name} for {app.Platform} v{app.Version} ({app.BuildNumber})";
        }

        private Task OnShowLicenses() => _navigation.Navigate<LicensesViewModel>();

        private Task OnShowProjectSite() => 
            _browser.ShowInBrowserAsync(new Uri("https://github.com/kipters/CrossNews"), true);

        public string VersionString { get; }

        public ICommand ShowProjectSiteCommand { get; }
        public ICommand ShowLicensesCommand { get; }
    }
}
