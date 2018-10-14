using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CrossNews.Core.Messages;
using CrossNews.Core.Model.Api;
using FireSharp;
using FireSharp.Config;
using MvvmCross.Plugin.Messenger;

namespace CrossNews.Core.Services
{
    public class NewsService : INewsService
    {
        private readonly IMvxMessenger _messenger;
        private readonly FirebaseClient _client;

        public NewsService(IMvxMessenger messenger)
        {
            _messenger = messenger;
            var config = new FirebaseConfig
            {
                BasePath = "https://hacker-news.firebaseio.com/v0/"
            };

            _client = new FirebaseClient(config);
        }

        public async Task<List<int>> GetStoryListAsync(StoryKind kind)
        {
            var listTag = GetStorylistTag(kind);

            var response = await _client.GetAsync(listTag);
            var items = response.ResultAs<List<int>>();

            return items;
        }

        public void EnqueueItems(List<int> ids)
        {
            var stopwatch = Stopwatch.StartNew();
            var tasks = ids.Select(id => _client.GetAsync($"item/{id}")
                .ContinueWith(task =>
                {
                    if (task.Status != TaskStatus.RanToCompletion)
                        return;
                    var storyItem = task.Result.ResultAs<Item>();
                    var msg = new NewsItemMessage<Item>(this, storyItem);
                    _messenger.Publish(msg);
                }));

            Task.WhenAll(tasks).ContinueWith(x =>
            {
                stopwatch.Stop();
                var ms = stopwatch.ElapsedMilliseconds;
                var msg = new DebugMessage(this, $"All completed in {ms} ms");
                _messenger.Publish(msg);
            });
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
