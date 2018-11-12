using System;
using CrossNews.Core;
using CrossNews.Core.Services;
using CrossNews.Ios.Services;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Platforms.Ios.Core;

namespace CrossNews.Ios
{
    public class Setup : MvxIosSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDialogService, IosDialogService>();
            Mvx.IoCProvider.RegisterSingleton<IBrowserService>(new IosBrowserService());
            Mvx.IoCProvider.RegisterSingleton<IShareService>(new IosShareService());
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IReachabilityService, IosReachabilityService>();
        }
    }
}
