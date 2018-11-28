using CrossNews.Core;
using CrossNews.Core.Services;
using CrossNews.Droid.Services;
using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.IoC;

namespace CrossNews.Droid
{
    public class Setup : MvxAppCompatSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            var ioc = Mvx.IoCProvider;
            ioc.RegisterSingleton<IAppService>(new DroidAppService());
            ioc.LazyConstructAndRegisterSingleton<IDialogService, DroidDialogService>();
            ioc.LazyConstructAndRegisterSingleton<IBrowserService, DroidBrowserService>();
            ioc.LazyConstructAndRegisterSingleton<IShareService, DroidShareService>();
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
            var ioc = Mvx.IoCProvider;
            ioc.ConstructAndRegisterSingleton<IReachabilityService, DroidReachabilityService>();
        }
    }
}