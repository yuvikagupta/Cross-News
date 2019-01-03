using System;
using CoreGraphics;
using CrossNews.Core.Extensions;
using CrossNews.Core.ViewModels;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using SafariServices;
using UIKit;

namespace CrossNews.Ios.Views
{
    [MvxChildPresentation]
    public class SettingsView : MvxViewController<SettingsViewModel>
        , IUITableViewDelegate
        , IUITableViewDataSource
    {
        private UITableView _tableView;

        public SettingsView(IntPtr handle) : base(handle) { }
        public SettingsView() { }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("stdCell", indexPath);
            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

            cell.TextLabel.Text = indexPath.Section == 0 
                ? "https://github.com/kipters/CrossNews" 
                : "Open source licenses";

            return cell;
        }

        public nint RowsInSection(UITableView tableView, nint section) => 1;

        [Export("numberOfSectionsInTableView:")]
        public nint NumberOfSections(UITableView tableView) => 2;

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Section == 0)
            {
                ViewModel.ShowProjectSiteCommand.TryExecute();
                return;
            }

            ViewModel.ShowLicensesCommand.TryExecute();
        }

        [Export("tableView:titleForHeaderInSection:")]
        public string TitleForHeader(UITableView tableView, nint section) => 
            section == 0 ? "Project site" : string.Empty;

        [Export("tableView:titleForFooterInSection:")]
        public string TitleForFooter(UITableView tableView, nint section) => 
            section == 1 ? ViewModel.VersionString : string.Empty;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Settings";

            var set = this.CreateBindingSet<SettingsView, SettingsViewModel>();

            if (ViewModel.ShowOverrideUi)
            {
                var overrideButton = new UIBarButtonItem("Features", UIBarButtonItemStyle.Done, null);
                set.Bind(overrideButton).To(vm => vm.ShowFeatureTogglesCommand).OneTime();
                NavigationItem.RightBarButtonItem = overrideButton;
            }

            set.Apply();

            _tableView = new UITableView(CGRect.Empty, UITableViewStyle.Grouped)
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                DataSource = this,
                Delegate = this
            };

            _tableView.RegisterClassForCellReuse(typeof(UITableViewCell), "stdCell");

            View.AddSubview(_tableView);

            _tableView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
            _tableView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
            _tableView.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            _tableView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;
        }

        public override void ViewWillAppear(bool animated)
        {
            var path = _tableView.IndexPathForSelectedRow;
            _tableView.DeselectRow(path, true);
        }
    }
}
