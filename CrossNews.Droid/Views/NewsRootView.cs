using Android.App;
using Android.Support.V7.Widget;
using CrossNews.Core.Extensions;
using CrossNews.Core.ViewModels;
using CrossNews.Droid.Common;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace CrossNews.Droid.Views
{
    [Activity(Theme = "@style/AppTheme.NoActionBar")]
    [MvxActivityPresentation]
    public class NewsRootView : MvxAppCompatActivity<NewsRootViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.newsroot_view);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            toolbar.Title = GetString(Resource.String.app_name);

            ViewModel.ShowInitialViewModelsCommand.TryExecute();
        }
    }
}
