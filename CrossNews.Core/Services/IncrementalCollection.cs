using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace CrossNews.Core.Services
{
    public class IncrementalCollection<T> : MvxObservableCollection<T>, IIncrementalCollection<T>
    {
        private readonly Func<int, Task<IList<T>>> _loadAction;
        public bool HasMoreItems { get; private set; } = true;

        public IncrementalCollection(Func<int, Task<IList<T>>> loadAction)
        {
            _loadAction = loadAction
                          ?? throw new ArgumentNullException(nameof(loadAction));
        }

        public async Task<int> LoadMoreItemsAsync(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Must be > 0");
            }

            if (count == 0)
            {
                return 0;
            }

            var result = await _loadAction(count);
            HasMoreItems = result != null;
            AddRange(result);

            return result?.Count ?? 0;
        }
    }
}