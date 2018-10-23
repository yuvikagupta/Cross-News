using MvvmCross.Base;
using MvvmCross.Tests;
using MvvmCross.Views;

namespace CrossNews.Core.Tests.ViewModels
{
    public abstract class ViewModelFixtureBase : MvxIoCSupportingTest
    {
        private MockDispatcher MockDispatcher { get; set; }

        protected ViewModelFixtureBase()
        {
            Setup();
            MockDispatcher = new MockDispatcher();
            Ioc.RegisterSingleton<IMvxViewDispatcher>(MockDispatcher);
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(MockDispatcher);
            Ioc.RegisterSingleton<IMvxMainThreadAsyncDispatcher>(MockDispatcher);
        }
    }
}