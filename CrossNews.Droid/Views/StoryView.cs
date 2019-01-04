using Android.App;
using Android.Views;
using Android.Webkit;
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
            SetContentView(Resource.Layout.story_view);

            Title = ViewModel.Title;

            var webView = FindViewById<WebView>(Resource.Id.webView);

            webView.LoadUrl(ViewModel.StoryUrl);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                OnBackPressed();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}
