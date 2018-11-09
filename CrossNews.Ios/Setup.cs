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
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IReachabilityService, IosReachabilityService>();
        }
    }
}
