using System.Threading.Tasks;
using CrossNews.Core.Services;
using CrossNews.Core.ViewModels;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CrossNews.Core
{
    public class CrossNewsAppStart : MvxAppStart
    {
        private readonly IFeatureStore _featureStore;

        public CrossNewsAppStart(IMvxApplication application, IMvxNavigationService navigationService, IFeatureStore featureStore)
            : base(application, navigationService) => _featureStore = featureStore;

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            var tabs = _featureStore.IsEnabled(Features.StoryTabPresentation);

            return tabs
                ? NavigationService.Navigate<NewsRootViewModel>()
                : NavigationService.Navigate<TopNewsViewModel>();
        }
    }
}
