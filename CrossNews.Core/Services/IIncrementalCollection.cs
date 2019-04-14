using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrossNews.Core.Services
{
    public interface IIncrementalSource
    {
        bool HasMoreItems { get; }
        Task<int> LoadMoreItemsAsync(int count);
    }

    public interface IIncrementalCollection<T> : IIncrementalSource, ICollection<T>
    {
    }
}
