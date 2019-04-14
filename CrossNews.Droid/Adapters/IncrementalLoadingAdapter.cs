using System.Collections;
using System.Collections.Specialized;
using System.Threading;
using Android.App;
using Android.Support.V7.Widget;
using CrossNews.Core.Services;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace CrossNews.Droid.Adapters
{
    public class IncrementalLoadingAdapter : MvxRecyclerAdapter
    {
        private int updateLock;

        public IncrementalLoadingAdapter(IMvxAndroidBindingContext bindingContext)
            : base(bindingContext)
        {
        }

        protected override void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsSourceCollectionChanged(sender, e);
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Interlocked.Exchange(ref updateLock, 0);
            }
        }

        private IIncrementalSource IncrementalSource { get; set; }

        public override IEnumerable ItemsSource
        {
            get => base.ItemsSource;
            set
            {
                base.ItemsSource = value;

                if (value is IIncrementalSource incremental)
                {
                    IncrementalSource = incremental;
                }
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            base.OnBindViewHolder(holder, position);

            if (position < ItemCount - 5 || ItemsSource == null)
            {
                return;
            }

            if (!IncrementalSource.HasMoreItems)
            {
                return;
            }

            var lockValue = Interlocked.Increment(ref updateLock);

            if (lockValue > 1)
            {
                return;
            }

            Application.SynchronizationContext.Post(
                _ => IncrementalSource.LoadMoreItemsAsync(30),
                null
            );
        }
    }
}
