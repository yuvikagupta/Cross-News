﻿using CrossNews.Core.Services;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;

namespace CrossNews.Core.ViewModels
{
    public class RecentNewsViewModel : NewsViewModel
    {
        public RecentNewsViewModel(IMvxNavigationService navigation
            , IMvxMessenger messenger
            , INewsService news
            , IReachabilityService reachability
            , IFeatureStore featureStore
            , IBrowserService browser
            , IDialogService dialog)
            : base(navigation, messenger, news, reachability, featureStore, browser, dialog)
        {
        }

        protected override StoryKind StoryKind { get; } = StoryKind.New;
    }
}
