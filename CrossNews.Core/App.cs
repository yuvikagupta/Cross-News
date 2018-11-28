using CrossNews.Core.Services;
using CrossNews.Core.ViewModels;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace CrossNews.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            var ioc = Mvx.IoCProvider;

            ioc.RegisterSingleton<IPlatformLicenseList>(new DummyPlatformLicenseList());
            ioc.RegisterSingleton<IPlatformFeatureOverrides>(new DummyPlatformOverrides());
            ioc.LazyConstructAndRegisterSingleton<IFeatureStore, FeatureStoreService>();
            ioc.RegisterSingleton<ICacheService>(new InMemoryCacheService());
            ioc.LazyConstructAndRegisterSingleton<INewsService, NewsService>();

            RegisterAppStart<TopNewsViewModel>();
        }
    }
}
