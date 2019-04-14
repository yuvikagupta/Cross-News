using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CrossNews.Core.Messages;
using CrossNews.Core.Model.Api;
using MvvmCross.Plugin.Messenger;
using Newtonsoft.Json;

namespace CrossNews.Core.Services
{
    public class CrudeNewsService : INewsService
    {
        private readonly IMvxMessenger _messenger;
        private readonly ICacheService _cache;
        private readonly HttpClient _client;

        public CrudeNewsService(IMvxMessenger messenger, ICacheService cache)
        {
            _messenger = messenger;
            _cache = cache;
            _client = new HttpClient { BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/") };
        }

        public async Task<List<int>> GetStoryListAsync(StoryKind kind)
        {
            var listTag = GetStorylistTag(kind);

            var response = await _client.GetStringAsync(listTag + ".json");
            var items = JsonConvert.DeserializeObject<List<int>>(response);

            return items;
        }

        public IEnumerable<Item> EnqueueItems(List<int> ids)
        {
            var (items, misses) = _cache.GetCachedItems(ids);
            var newItems = new ConcurrentBag<Item>();
            var tasks = misses.Select(GetAndPublishItemAsync);

            var itemList = items.ToList();
            foreach (var item in itemList)
            {
                var msg = new NewsItemMessage(this, item);
                _messenger.Publish(msg);
            }

            Task.WhenAll(tasks).ContinueWith(x =>
            {
                _cache.AddItemsToCache(newItems);
            });

            return itemList;

            async Task GetAndPublishItemAsync(int id)
            {
                var data = await _client.GetStringAsync($"item/{id}.json");
                var storyItem = JsonConvert.DeserializeObject<Item>(data);
                newItems.Add(storyItem);
                var msg = new NewsItemMessage(this, storyItem);
                _messenger.Publish(msg);
            }
        }

        private string GetStorylistTag(StoryKind kind)
        {
            switch (kind)
            {
                case StoryKind.Top:
                    return "topstories";
                case StoryKind.New:
                    return "newstories";
                case StoryKind.Best:
                    return "beststories";
                case StoryKind.Ask:
                    return "askstories";
                case StoryKind.Show:
                    return "showstories";
                case StoryKind.Job:
                    return "jobstories";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }
    }
}
