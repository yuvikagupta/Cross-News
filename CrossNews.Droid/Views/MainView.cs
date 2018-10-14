using Android.App;
using CrossNews.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace CrossNews.Droid.Views
{
    [Activity]
    [MvxActivityPresentation]
    public class MainView : MvxAppCompatActivity<MainViewModel>
    {
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.main_view);
        }
    }
}