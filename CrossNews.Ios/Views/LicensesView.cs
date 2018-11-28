using System;
using CoreGraphics;
using CrossNews.Core.Extensions;
using CrossNews.Core.ViewModels;
using Foundation;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace CrossNews.Ios.Views
{
    public class LicensesView : MvxViewController<LicensesViewModel>
        , IUITableViewDelegate
        , IUITableViewDataSource
    {
        private UITableView _tableView;

        public LicensesView() { }
        public LicensesView(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Open source licenses";

            _tableView = new UITableView(CGRect.Empty, UITableViewStyle.Grouped)
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Delegate = this,
                DataSource = this
            };

            View.AddSubview(_tableView);

            _tableView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
            _tableView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
            _tableView.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            _tableView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("stdCell")
                ?? new UITableViewCell(UITableViewCellStyle.Value1, "stdCell");

            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

            var item = indexPath.Section == 0 
                ? ViewModel.CoreLicenses[(int) indexPath.Item]
                : ViewModel.PlatformLicenses[(int) indexPath.Item];

            cell.TextLabel.Text = item.Name;
            cell.DetailTextLabel.Text = item.LicenseName;

            return cell;
        }

        [Export("numberOfSectionsInTableView:")]
        public nint NumberOfSections(UITableView tableView) => ViewModel.PlatformLicenses == null ? 1 : 2;

        public nint RowsInSection(UITableView tableView, nint section)
        {
            return section == 0
                ? ViewModel.CoreLicenses.Count
                : ViewModel.PlatformLicenses.Count;
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var item = ViewModel.CoreLicenses[(int)indexPath.Item];

            ViewModel.ShowLicense.TryExecute(item);
        }

        [Export("tableView:titleForHeaderInSection:")]
        public string TitleForHeader(UITableView tableView, nint section)
        {
            return section == 0
                ? "Core dependencies"
                : "App dependencies";
        }

        public override void ViewWillAppear(bool animated)
        {
            var path = _tableView.IndexPathForSelectedRow;
            _tableView.DeselectRow(path, true);
        }
    }
}
