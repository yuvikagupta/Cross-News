using System.Collections.Generic;
using CrossNews.Core.Model.Api;

namespace CrossNews.Core.Services
{
    public interface ICacheService
    {
        (IEnumerable<Item> items, IEnumerable<int> misses) GetCachedItems(IEnumerable<int> ids);
        void AddItemsToCache(IEnumerable<Item> items);
    }
}
