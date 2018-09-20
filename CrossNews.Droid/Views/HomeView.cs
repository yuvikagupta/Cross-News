using Android.App;
using CrossNews.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace CrossNews.Droid.Views
{
    [MvxActivityPresentation]
    [Activity]
    public class HomeView : MvxAppCompatActivity<HomeViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.home_view);
        }
    }
}