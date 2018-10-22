using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CrossNews.Core.Messages;
using CrossNews.Core.Model.Api;
using MvvmCross.Plugin.Messenger;
using Newtonsoft.Json;

namespace CrossNews.Core.Services
{
    public class NewsService : INewsService
    {
        private readonly IMvxMessenger _messenger;
        private readonly ICacheService _cache;
        private readonly HttpClient _client;

        public NewsService(IMvxMessenger messenger, ICacheService cache)
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
            var stopwatch = Stopwatch.StartNew();

            var (items, misses) = _cache.GetCachedItems(ids);

            var newItems = new List<Item>();

            var tasks = misses.Select(id => _client.GetStringAsync($"item/{id}.json")
                .ContinueWith(task =>
                {
                    if (task.Status != TaskStatus.RanToCompletion)
                        return;
                    var storyItem = JsonConvert.DeserializeObject<Item>(task.Result);
                    newItems.Add(storyItem);
                    var msg = new NewsItemMessage<Item>(this, storyItem);
                    _messenger.Publish(msg);
                }));

            foreach (var item in items)
            {
                var msg = new NewsItemMessage<Item>(this, item);
                _messenger.Publish(msg);
            }

            Task.WhenAll(tasks).ContinueWith(x =>
            {
                _cache.AddItemsToCache(newItems);
                stopwatch.Stop();
                var ms = stopwatch.ElapsedMilliseconds;
                Debug.WriteLine("Queue completed in {0} ms", ms);
            });

            return items;
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
