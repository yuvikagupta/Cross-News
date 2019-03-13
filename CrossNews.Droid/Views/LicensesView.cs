using System.Collections.Generic;
using Android.App;
using Android.Support.V7.Widget;
using Android.Widget;
using CrossNews.Core.Model;
using CrossNews.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace CrossNews.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Label = "Open source licenses")]
    public class LicensesView : MvxAppCompatActivity<LicensesViewModel>
    {
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.licenses_view);

            var listView = FindViewById<MvxExpandableListView>(Resource.Id.licenses_list);

            var groups = new List<ListGroup<LicenseInfo>>
            {
                new ListGroup<LicenseInfo>("Core dependencies", ViewModel.CoreLicenses),
                new ListGroup<LicenseInfo>("App dependencies", ViewModel.PlatformLicenses)
            };

            listView.ItemsSource = groups;
        }
    }

    public class ListGroup<T> : List<T>
    {
        public ListGroup(string title, IEnumerable<T> collection) : base(collection)
        {
            Title = title;
        }
        public string Title { get; }
    }
}
