using CrossNews.Core.Services;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;

namespace CrossNews.Core.ViewModels
{
    public class JobsNewsViewModel : NewsViewModel
    {
        public JobsNewsViewModel(IMvxNavigationService navigation
            , IMvxMessenger messenger
            , INewsService news
            , IReachabilityService reachability
            , IFeatureStore featureStore
            , IBrowserService browser
            , IDialogService dialog
            , IIncrementalCollectionFactory incrementalCollectionFactory)
            : base(navigation, messenger, news, reachability, featureStore, browser, dialog, incrementalCollectionFactory)
        {
        }

        protected override StoryKind StoryKind { get; } = StoryKind.Job;
    }
}
