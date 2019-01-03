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
        private readonly IFeatureStore _features;

        public SettingsViewModel(IMvxNavigationService navigation, IBrowserService browser, IAppService app, IFeatureStore features)
        {
            _navigation = navigation;
            _browser = browser;
            _features = features;
            ShowProjectSiteCommand = new MvxAsyncCommand(OnShowProjectSite);
            ShowLicensesCommand = new MvxAsyncCommand(OnShowLicenses);
            ShowFeatureTogglesCommand = new MvxAsyncCommand(OnShowFeatureToggles, () => features.IsEnabled(Features.ShowOverrideUi));

            VersionString = $"{app.Name} for {app.Platform} v{app.Version} ({app.BuildNumber})";
        }

        private Task OnShowLicenses() => _navigation.Navigate<LicensesViewModel>();

        private Task OnShowProjectSite() => 
            _browser.ShowInBrowserAsync(new Uri("https://github.com/kipters/CrossNews"), true);

        private Task OnShowFeatureToggles() => _navigation.Navigate<FeatureTogglesViewModel>();

        public bool ShowOverrideUi => _features.IsEnabled(Features.ShowOverrideUi);
        public string VersionString { get; }

        public ICommand ShowProjectSiteCommand { get; }
        public ICommand ShowLicensesCommand { get; }
        public ICommand ShowFeatureTogglesCommand { get; }
    }
}
