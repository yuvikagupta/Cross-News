using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrossNews.Core.Extensions;
using CrossNews.Core.Messages;
using CrossNews.Core.Model.Api;
using CrossNews.Core.Services;
using CrossNews.Core.ViewModels;
using Moq;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Xunit;

namespace CrossNews.Core.Tests.ViewModels
{
    public class TopNewsViewModelTests : ViewModelFixtureBase
    {
        private Mock<IMvxNavigationService> Navigation => new Mock<IMvxNavigationService>();
        private Mock<IMvxMessenger> Messenger => new Mock<IMvxMessenger>();
        private Mock<INewsService> News => new Mock<INewsService>();
        private Mock<IReachabilityService> Reachability
        {
            get
            {
                var mock = new Mock<IReachabilityService>();
                mock.Setup(r => r.IsConnectionAvailable).Returns(true);
                return mock;
            }
        }

        [Fact]
        public async Task ShowErrorMessageIfNetworkIsDown()
        {
            var reachability = new Mock<IReachabilityService>();
            reachability.Setup(r => r.IsConnectionAvailable)
                .Returns(false)
                .Verifiable();

            var sut = new TopNewsViewModel(Navigation.Object, Messenger.Object, News.Object, reachability.Object);

            await sut.Initialize();
            sut.ViewCreated();

            reachability.VerifyGet(r => r.IsConnectionAvailable, Times.Once);
            Assert.True(sut.LoadingTask.IsFaulted);
            Assert.False(string.IsNullOrWhiteSpace(sut.LoadingTask.ErrorMessage));
        }

        [Fact]
        public async Task DontRequestNewsIfNetworkIsDown()
        {
            var reachability = new Mock<IReachabilityService>();
            reachability.Setup(r => r.IsConnectionAvailable)
                .Returns(false);

            var news = News;
            news.Setup(n => n.GetStoryListAsync(It.IsAny<StoryKind>()))
                .ReturnsAsync(Enumerable.Range(0, 20).ToList())
                .Verifiable();

            var sut = new TopNewsViewModel(Navigation.Object, Messenger.Object, news.Object, reachability.Object);

            await sut.Initialize();
            sut.ViewCreated();

            news.Verify(n => n.GetStoryListAsync(It.IsAny<StoryKind>()), Times.Never);
        }

        [Fact]
        public async Task ShowErrorMessageIfFetchFails()
        {
            var news = News;
            news.Setup(n => n.GetStoryListAsync(It.IsAny<StoryKind>()))
                .Throws<Exception>()
                .Verifiable();

            var sut = new TopNewsViewModel(Navigation.Object, Messenger.Object, news.Object, Reachability.Object);

            await sut.Initialize();
            sut.ViewCreated();

            news.Verify(n => n.GetStoryListAsync(It.IsAny<StoryKind>()), Times.Once);
            Assert.True(sut.LoadingTask.IsFaulted);
            Assert.False(string.IsNullOrWhiteSpace(sut.LoadingTask.ErrorMessage));
        }

        [Fact]
        public async Task PrepareStoriesWithUnfilledItems()
        {
            var items = Enumerable.Range(0, 20).ToList();
            var news = News;
            news.Setup(n => n.GetStoryListAsync(It.IsAny<StoryKind>()))
                .ReturnsAsync(items)
                .Verifiable();

            var sut = new TopNewsViewModel(Navigation.Object, Messenger.Object, news.Object, Reachability.Object);

            await sut.Initialize();
            sut.ViewCreated();

            var ids = sut.Stories.Select(x => x.Id).ToList();
            Assert.All(sut.Stories, vm => Assert.Contains(vm.Id, items));
            Assert.All(items, item => Assert.Contains(item, ids));
            Assert.True(sut.LoadingTask.IsCompleted);
            news.Verify(n => n.GetStoryListAsync(It.IsAny<StoryKind>()), Times.Once);
        }

        [Fact]
        public async Task FillItemsAsTheyCome()
        {
            Action<NewsItemMessage<Item>> callback = null;
            var messenger = new Mock<IMvxMessenger>();
            messenger.Setup(m => m.Subscribe(It.IsAny<Action<NewsItemMessage<Item>>>(), It.IsAny<MvxReference>(), It.IsAny<string>()))
                .Callback((Action<NewsItemMessage<Item>> action, MvxReference mvxref, string tag) => callback = action)
                .Returns(new MvxSubscriptionToken(Guid.NewGuid(), () => { }))
                .Verifiable();

            var items = Enumerable.Range(0, 20).ToList();
            var news = News;
            news.Setup(n => n.GetStoryListAsync(It.IsAny<StoryKind>()))
                .ReturnsAsync(items)
                .Verifiable();

            var sut = new TopNewsViewModel(Navigation.Object, messenger.Object, news.Object, Reachability.Object);

            await sut.Initialize();
            sut.ViewCreated();

            messenger.Verify(m => m.Subscribe(It.IsAny<Action<NewsItemMessage<Item>>>(), It.IsAny<MvxReference>(), It.IsAny<string>()), Times.Once);
            news.Verify(n => n.GetStoryListAsync(It.IsAny<StoryKind>()), Times.Once);
            Assert.True(sut.LoadingTask.IsCompleted);
            Assert.NotNull(callback);

            foreach (var item in items)
            {
                var story = new Item {Id = item, Title = $"Item {item}"};
                callback(new NewsItemMessage<Item>(news.Object, story));
            }

            Assert.All(sut.Stories, vm => Assert.Equal($"Item {vm.Id}", vm.Title));
        }

        [Fact] // Temporary
        public async Task ShouldOnlyShowStories()
        {
            var item = new Item { Id = 99, Type = ItemType.Job};
            IStory paramItem = null;
            var navigation = Navigation;
            navigation.Setup(n => n.Navigate<StoryViewModel, IStory>(item, It.IsAny<IMvxBundle>(), It.IsAny<CancellationToken>()))
                .Callback((IStory param, IMvxBundle bundle, CancellationToken token) => paramItem = param)
                .ReturnsAsync(true)
                .Verifiable();

            var news = News;
            news.Setup(n => n.GetStoryListAsync(It.IsAny<StoryKind>()))
                .ReturnsAsync(new List<int> { item.Id });

            Action<NewsItemMessage<Item>> callback = null;
            var messenger = new Mock<IMvxMessenger>();
            messenger.Setup(m => m.Subscribe(It.IsAny<Action<NewsItemMessage<Item>>>(), It.IsAny<MvxReference>(), It.IsAny<string>()))
                .Callback((Action<NewsItemMessage<Item>> action, MvxReference mvxref, string tag) => callback = action)
                .Returns(new MvxSubscriptionToken(Guid.NewGuid(), () => { }));

            var sut = new TopNewsViewModel(navigation.Object, messenger.Object, news.Object, Reachability.Object);

            await sut.Initialize();
            sut.ViewCreated();

            callback(new NewsItemMessage<Item>(news.Object, item));

            sut.ShowStoryCommand.TryExecute(sut.Stories[0]);

            Assert.False(sut.ShowStoryCommand.CanExecute(sut.Stories[0]));
            navigation.Verify(n => n.Navigate<StoryViewModel, IStory>(It.IsAny<IStory>(), It.IsAny<MvxBundle>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task TappingAnItemShowsIt()
        {
            var item = new Item {Id = 99, Type = ItemType.Story};
            IStory paramItem = null;
            var navigation = Navigation;
            navigation.Setup(n => n.Navigate<StoryViewModel, IStory>(item, It.IsAny<IMvxBundle>(), It.IsAny<CancellationToken>()))
                .Callback((IStory param, IMvxBundle bundle, CancellationToken token) => paramItem = param)
                .ReturnsAsync(true)
                .Verifiable();

            var news = News;
            news.Setup(n => n.GetStoryListAsync(It.IsAny<StoryKind>()))
                .ReturnsAsync(new List<int> {item.Id});

            Action<NewsItemMessage<Item>> callback = null;
            var messenger = new Mock<IMvxMessenger>();
            messenger.Setup(m => m.Subscribe(It.IsAny<Action<NewsItemMessage<Item>>>(), It.IsAny<MvxReference>(), It.IsAny<string>()))
                .Callback((Action<NewsItemMessage<Item>> action, MvxReference mvxref, string tag) => callback = action)
                .Returns(new MvxSubscriptionToken(Guid.NewGuid(), () => { }));

            var sut = new TopNewsViewModel(navigation.Object, messenger.Object, news.Object, Reachability.Object);

            await sut.Initialize();
            sut.ViewCreated();

            callback(new NewsItemMessage<Item>(news.Object, item));

            sut.ShowStoryCommand.TryExecute(sut.Stories[0]);

            navigation.Verify();
            Assert.Same(item, paramItem);
        }

        [Fact]
        public async Task ShouldNotShowUnfilledItemsOnTap()
        {
            var navigation = Navigation;
            navigation.Setup(n => n.Navigate<StoryViewModel, IStory>(It.IsAny<IStory>(), It.IsAny<MvxBundle>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .Verifiable();

            var news = News;
            news.Setup(n => n.GetStoryListAsync(It.IsAny<StoryKind>()))
                .ReturnsAsync(new List<int> {99});

            var sut = new TopNewsViewModel(navigation.Object, Messenger.Object, news.Object, Reachability.Object);

            await sut.Initialize();
            sut.ViewCreated();

            sut.ShowStoryCommand.TryExecute(sut.Stories[0]);

            Assert.False(sut.ShowStoryCommand.CanExecute(sut.Stories[0]));
            navigation.Verify(n => n.Navigate<StoryViewModel, IStory>(It.IsAny<IStory>(), It.IsAny<MvxBundle>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldPurgeAndReloadItemsOnRefresh()
        {
            var items = Enumerable.Range(0, 20).ToList();
            var news = News;
            news.Setup(n => n.GetStoryListAsync(It.IsAny<StoryKind>()))
                // ReSharper disable once AccessToModifiedClosure
                .ReturnsAsync(() => items)
                .Verifiable();

            var sut = new TopNewsViewModel(Navigation.Object, Messenger.Object, news.Object, Reachability.Object);

            await sut.Initialize();
            sut.ViewCreated();

            items = Enumerable.Range(0, 19).ToList();

            sut.RefreshCommand.TryExecute();

            var ids = sut.Stories.Select(x => x.Id).ToList();
            Assert.All(sut.Stories, vm => Assert.Contains(vm.Id, items));
            Assert.All(items, item => Assert.Contains(item, ids));
            Assert.DoesNotContain(sut.Stories, vm => vm.Id == 19);
            news.Verify(n => n.GetStoryListAsync(It.IsAny<StoryKind>()), Times.Exactly(2));
        }
    }
}