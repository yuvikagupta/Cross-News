using System;
using CrossNews.Core;
using CrossNews.Core.Services;
using CrossNews.Core.ViewModels;
using CrossNews.Ios.Services;
using CrossNews.Ios.Views;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Platforms.Ios.Views;

namespace CrossNews.Ios
{
    public class Setup : MvxIosSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            Mvx.IoCProvider.RegisterSingleton<IAppService>(new IosAppService());
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IPlatformFeatureOverlay, IosPlatformFeatureOverlay>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDialogService, IosDialogService>();
            Mvx.IoCProvider.RegisterSingleton<IBrowserService>(new IosBrowserService());
            Mvx.IoCProvider.RegisterSingleton<IShareService>(new IosShareService());
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
            Mvx.IoCProvider.RegisterSingleton<IPlatformLicenseList>(new IosPlatformLicenseList());
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IReachabilityService, IosReachabilityService>();
        }

        protected override IMvxIosViewsContainer CreateIosViewsContainer()
        {
            var container = base.CreateIosViewsContainer();

            container.Add<TopNewsViewModel, NewsView>();
            container.Add<RecentNewsViewModel, NewsView>();
            container.Add<AskNewsViewModel, NewsView>();
            container.Add<ShowNewsViewModel, NewsView>();
            container.Add<JobsNewsViewModel, NewsView>();

            return container;
        }
    }
}
