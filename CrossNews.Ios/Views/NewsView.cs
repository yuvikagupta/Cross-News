using System;
using CrossNews.Core.Services;
using CrossNews.Core.ViewModels;
using Foundation;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using UIKit;

namespace CrossNews.Ios.Views
{
    public class NewsView : MvxTableViewController<NewsViewModel>
        , IScrollableView
        , IMvxOverridePresentationAttribute
    {
        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            var features = Mvx.IoCProvider.Resolve<IFeatureStore>();

            var title = GetTitle(request.ViewModelType);
            var iconPrefix = GetIconPrefix(request.ViewModelType);

            return features.IsEnabled(Features.StoryTabPresentation)
                ? new MvxTabPresentationAttribute
                {
                    TabName = title,
                    WrapInNavigationController = false,
                    TabIconName = $"{iconPrefix}Icon",
                    TabSelectedIconName = $"{iconPrefix}Filled"
                }
                : (MvxBasePresentationAttribute)new MvxRootPresentationAttribute
                {
                    WrapInNavigationController = true
                };

            string GetIconPrefix(Type viewmodelType)
            {
                if (viewmodelType == typeof(AskNewsViewModel))
                {
                    return "AskHn";
                }

                if (viewmodelType == typeof(JobsNewsViewModel))
                {
                    return "Jobs";
                }

                if (viewmodelType == typeof(RecentNewsViewModel))
                {
                    return "New";
                }

                if (viewmodelType == typeof(ShowNewsViewModel))
                {
                    return "ShowHn";
                }

                if (viewmodelType == typeof(TopNewsViewModel))
                {
                    return "TopNews";
                }

                throw new ArgumentOutOfRangeException(nameof(viewmodelType), "Invalid news kind");
            }
        }

        // TODO replace with a proper localization service
        private string GetTitle(Type viewmodelType)
        {
            if (viewmodelType == typeof(AskNewsViewModel))
            {
                return "Ask HN";
            }

            if (viewmodelType == typeof(JobsNewsViewModel))
            {
                return "Jobs";
            }

            if (viewmodelType == typeof(RecentNewsViewModel))
            {
                return "New";
            }

            if (viewmodelType == typeof(ShowNewsViewModel))
            {
                return "Show HN";
            }

            if (viewmodelType == typeof(TopNewsViewModel))
            {
                return "Top News";
            }

            throw new ArgumentOutOfRangeException(nameof(viewmodelType), "Invalid news kind");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;
            Title = GetTitle(ViewModel.GetType());

            var source = new MvxStandardTableViewSource(TableView, UITableViewCellStyle.Subtitle, (NSString)"stdCell",
                "TitleText Title; DetailText 'Posted by ' + Author + ' • ' + Score + ' points • ' + CommentsCount + ' comments'");

            TableView.Source = source;

            var refreshControl = new MvxUIRefreshControl();

            var set = this.CreateBindingSet<NewsView, NewsViewModel>();

            if (!ViewModel.TabPresentation)
            {
#pragma warning disable XI0001 // Notifies you with advices on how to use Apple APIs
                var settingsIcon = UIImage.FromBundle("SettingsIcon");
                var settingsButton = new UIBarButtonItem { Image = settingsIcon };
                NavigationItem.RightBarButtonItem = settingsButton;
#pragma warning restore XI0001 // Notifies you with advices on how to use Apple APIs

                set.Bind(settingsButton)
                   .To(vm => vm.ShowSettingsCommand)
                   .OneTime();
            }

            set.Bind(source)
               .To(vm => vm.Stories)
               .OneTime();

            set.Bind(source)
               .For(s => s.SelectionChangedCommand)
               .To(vm => vm.ShowStoryCommand)
               .OneTime();

            set.Bind(refreshControl)
               .For(v => v.RefreshCommand)
               .To(vm => vm.RefreshCommand)
               .OneTime();

            set.Bind(refreshControl)
               .For(v => v.IsRefreshing)
               .To(vm => vm.IsBusy)
               .OneWay();

            set.Apply();

            RefreshControl = refreshControl;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

#pragma warning disable XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                NavigationController.NavigationBar.PrefersLargeTitles = true;
            }
#pragma warning restore XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version
        }

        public void ScrollToTop() => 
            TableView.ScrollToRow(NSIndexPath.FromRowSection(0, 0), UITableViewScrollPosition.Top, true);
    }
}
