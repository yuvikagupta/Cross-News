using Android.App;
using CrossNews.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace CrossNews.Droid.Views
{
    [Activity, MvxActivityPresentation]
    public class StoryView : MvxAppCompatActivity<StoryViewModel>
    {
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
        }
    }
}