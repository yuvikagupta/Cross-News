using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CrossNews.Core.Model.Api;

namespace CrossNews.Core.Services
{
    class InMemoryCacheService : ICacheService
    {
        private readonly IDictionary<int, (Item item, DateTime expiration)> _store;

        public InMemoryCacheService()
        {
            _store = new ConcurrentDictionary<int, (Item item, DateTime expiration)>();
        }

        public (IEnumerable<Item> items, IEnumerable<int> misses) GetCachedItems(IEnumerable<int> ids)
        {
            var idList = ids as List<int> ?? ids.ToList();

            var items = _store
                .Where(p => p.Value.expiration >= DateTime.UtcNow)
                .Join(idList, pair => pair.Key, i => i, (pair, i) => pair.Value)
                .Select(p => p.item);

            var cachedItems = items.ToList();

            var misses = idList.Except(cachedItems.Select(i => i.Id));

            return (cachedItems, misses);
        }

        public void AddItemsToCache(IEnumerable<Item> items)
        {
            var expiration = DateTime.UtcNow.AddMinutes(10);
            foreach (var item in items)
            {
                _store[item.Id] = (item, expiration);
            }
        }
    }
}