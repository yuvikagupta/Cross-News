using Android.App;
using CrossNews.Core.ViewModels;
using CrossNews.Droid.Common;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace CrossNews.Droid.Views
{
    [Activity(Label = "Top News"), MvxActivityPresentation]
    public class TopNewsView : MvxAppCompatActivity<TopNewsViewModel>
    {
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.stories_view);

            var recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.storiesRecyclerView);
            recyclerView.AddItemDecoration(new LineDividerItemDecoration(this));
        }
    }
}