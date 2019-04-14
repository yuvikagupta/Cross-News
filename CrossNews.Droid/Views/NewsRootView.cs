using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using CrossNews.Core.Extensions;
using CrossNews.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_settings:
                    ViewModel.ShowSettingsCommand.TryExecute();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
