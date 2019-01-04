using MvvmCross.Base;
using MvvmCross.Binding;
using MvvmCross.Tests;
using MvvmCross.Views;

namespace CrossNews.Core.Tests.ViewModels
{
    public abstract class ViewModelFixtureBase : MvxIoCSupportingTest
    {
        private MockDispatcher MockDispatcher { get; }

        private static bool _init;
        private static readonly object InitLock = new object();
        
        protected ViewModelFixtureBase()
        {
            lock (InitLock)
            {
                if (_init)
                {
                    return;
                }

                Setup();
                MockDispatcher = new MockDispatcher();
                Ioc.RegisterSingleton<IMvxViewDispatcher>(MockDispatcher);
                Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(MockDispatcher);
                Ioc.RegisterSingleton<IMvxMainThreadAsyncDispatcher>(MockDispatcher);
                _init = true;
            }
        }
    }
}