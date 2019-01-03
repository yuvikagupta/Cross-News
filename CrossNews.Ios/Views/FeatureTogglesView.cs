using CrossNews.Core.ViewModels;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace CrossNews.Ios.Views
{
    [MvxChildPresentation]
    public class FeatureTogglesView : MvxTableViewController<FeatureTogglesViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;
            Title = "Feature toggles";

            var set = this.CreateBindingSet<FeatureTogglesView, FeatureTogglesViewModel>();

            var source = new MvxStandardTableViewSource(TableView, UITableViewCellStyle.Value1, (NSString)"stdCell", "TitleText Key; DetailText BoolToOnOff(Value)");

            set.Bind(source).To(vm => vm.Toggles).OneTime();

            set.Apply();

            TableView.Source = source;
            TableView.AllowsSelection = false;
        }
    }
}
