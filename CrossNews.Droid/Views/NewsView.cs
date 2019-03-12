using Android.OS;
using Android.Views;
using CrossNews.Core.ViewModels;
using CrossNews.Droid.Common;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace CrossNews.Droid.Views
{
    public abstract class NewsView<T> : MvxFragment<T> where T : NewsViewModel
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.stories_view, null);
            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.storiesRecyclerView);
            recyclerView.AddItemDecoration(new LineDividerItemDecoration(Activity));

            return view;
        }
    }

    [MvxTabLayoutPresentation("Top News", Resource.Id.viewpager, Resource.Id.tabs)]
    public class TopNewsView : NewsView<TopNewsViewModel>
    {
    }

    [MvxTabLayoutPresentation("New", Resource.Id.viewpager, Resource.Id.tabs)]
    public class RecentNewsView : NewsView<RecentNewsViewModel>
    {
    }

    [MvxTabLayoutPresentation("Ask HN", Resource.Id.viewpager, Resource.Id.tabs)]
    public class AskNewsView : NewsView<AskNewsViewModel>
    {
    }

    [MvxTabLayoutPresentation("Show HN", Resource.Id.viewpager, Resource.Id.tabs)]
    public class ShowNewsView : NewsView<ShowNewsViewModel>
    {
    }

    [MvxTabLayoutPresentation("Jobs", Resource.Id.viewpager, Resource.Id.tabs)]
    public class JobsNewsView : NewsView<JobsNewsViewModel>
    {
    }
}
