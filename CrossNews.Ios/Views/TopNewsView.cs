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

            var source = new MvxStandardTableViewSource(TableView, UITableViewCellStyle.Value2, (NSString)"stdCell", "DetailText Title; TitleText Score");
            TableView.Source = source;
            var refreshButton = new UIBarButtonItem(UIBarButtonSystemItem.Refresh);

            NavigationItem.RightBarButtonItem = refreshButton;

            var set = this.CreateBindingSet<TopNewsView, TopNewsViewModel>();

            set.Bind(source).To(vm => vm.Stories).OneTime();
            set.Bind(refreshButton).To(vm => vm.RefreshCommand).OneTime();
            set.Apply();
        }
    }
}
