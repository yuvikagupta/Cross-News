using CrossNews.Core.ViewModels;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace CrossNews.Ios.Views
{
    [MvxRootPresentation(WrapInNavigationController = true)]
    public class TopNewsView : MvxTableViewController<TopNewsViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            Title = "Top news";

#pragma warning disable XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                NavigationController.NavigationBar.PrefersLargeTitles = true;
#pragma warning restore XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version

            var source = new MvxStandardTableViewSource(TableView, UITableViewCellStyle.Subtitle, (NSString)"stdCell", 
                "TitleText Title; DetailText 'Posted by ' + Author + ' | ' + Score + ' points | ' + CommentsCount + ' comments'");

            TableView.Source = source;

            var refreshControl = new MvxUIRefreshControl();

            var set = this.CreateBindingSet<TopNewsView, TopNewsViewModel>();

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
    }
}
