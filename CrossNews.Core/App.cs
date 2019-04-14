using CrossNews.Core.Services;
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
            ioc.RegisterSingleton<IFeatureProvider>(new FeatureProvider());
            ioc.LazyConstructAndRegisterSingleton<IFeatureStore, FeatureStoreService>();
            ioc.RegisterSingleton<ICacheService>(new InMemoryCacheService());
            ioc.LazyConstructAndRegisterSingleton<INewsService, CrudeNewsService>();
            ioc.RegisterSingleton<IIncrementalCollectionFactory>(new IncrementalCollectionFactory());

            RegisterCustomAppStart<CrossNewsAppStart>();
        }
    }
}
