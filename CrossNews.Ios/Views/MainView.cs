using System;
using System.Collections.Specialized;
using System.ComponentModel;
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
    public class MainView : MvxTableViewController<MainViewModel>
    {
        protected internal MainView(IntPtr handle) : base(handle) { }

        public MainView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.PropertyChanged += OnPropertyChanged;

            Title = "Top stories";

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                NavigationController.NavigationBar.PrefersLargeTitles = true;

            View.BackgroundColor = UIColor.White;
            var reloadBtn = new UIBarButtonItem(UIBarButtonSystemItem.Refresh);
            NavigationItem.RightBarButtonItem = reloadBtn;

            var source = new MvxStandardTableViewSource(TableView, UITableViewCellStyle.Subtitle, (NSString)"stdCell", "TitleText Title; DetailText Index + '-' + Id");
            TableView.Source = source;

            var set = this.CreateBindingSet<MainView, MainViewModel>();

            set.Bind(source).To(vm => vm.Stories).OneTime();
            set.Bind(source).For(v => v.SelectionChangedCommand).To(vm => vm.ShowStoryCommand).OneTime();
            set.Bind(reloadBtn).To(vm => vm.ReloadCommand).OneTime();

            set.Apply();
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Stories))
            {
                ViewModel.Stories.CollectionChanged -= OnCollectionChanged;
                ViewModel.Stories.CollectionChanged += OnCollectionChanged;
            }
        }
    }
}
