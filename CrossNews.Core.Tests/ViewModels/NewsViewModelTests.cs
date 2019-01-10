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
using F = CrossNews.Core.Services.Features;

namespace CrossNews.Core.Tests.ViewModels
{
    internal class MockNewsViewModel : NewsViewModel
    {
        private StoryKind _storyKind;

        public MockNewsViewModel(IMvxNavigationService navigation
            , IMvxMessenger messenger
            , INewsService news
            , IReachabilityService reachability
            , IFeatureStore featureStore
            , IBrowserService browser
            , IDialogService dialog)
            : base(navigation, messenger, news, reachability, featureStore, browser, dialog) => 
            _storyKind = StoryKind.Best;

        protected override StoryKind StoryKind => _storyKind;
        public void SetStoryKind(StoryKind storyKind) => _storyKind = storyKind;
    }

    public class NewsViewModelTests : ViewModelFixtureBase
    {
        private Mock<IMvxNavigationService> Navigation => new Mock<IMvxNavigationService>();
        private Mock<IMvxMessenger> Messenger => new Mock<IMvxMessenger>();
        private Mock<INewsService> News => new Mock<INewsService>();
        private Mock<IDialogService> Dialog => new Mock<IDialogService>();
        private Mock<IReachabilityService> Reachability
        {
            get
            {
                var mock = new Mock<IReachabilityService>();
                mock.Setup(r => r.IsConnectionAvailable).Returns(true);
                return mock;
            }
        }
        private Mock<IFeatureStore> Features = new Mock<IFeatureStore>();
        private Mock<IBrowserService> Browser = new Mock<IBrowserService>();

        [Fact]
        public async Task ShowErrorMessageIfNetworkIsDown()
        {
            var reachability = new Mock<IReachabilityService>();
            reachability.Setup(r => r.IsConnectionAvailable)
                .Returns(false)
                .Verifiable();

            var sut = new MockNewsViewModel(Navigation.Object, Messenger.Object, News.Object, reachability.Object, Features.Object, Browser.Object, Dialog.Object);

            await sut.Initialize();
            sut.ViewCreated();
                
            reachability.VerifyGet(r => r.IsConnectionAvailable, Times.Once);
            Assert.True(sut.LoadingTask.IsCompleted);
            Assert.False(sut.LoadingTask.IsNotCompleted);
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

            var sut = new MockNewsViewModel(Navigation.Object, Messenger.Object, news.Object, reachability.Object, Features.Object, Browser.Object, Dialog.Object);

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

            var sut = new MockNewsViewModel(Navigation.Object, Messenger.Object, news.Object, Reachability.Object, Features.Object, Browser.Object, Dialog.Object);

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

            var sut = new MockNewsViewModel(Navigation.Object, Messenger.Object, news.Object, Reachability.Object, Features.Object, Browser.Object, Dialog.Object);

            await sut.Initialize();
            sut.ViewCreated();

            var ids = sut.Stories.Select(x => x.Id).ToList();
            Assert.All(sut.Stories, vm => Assert.Contains(vm.Id, items));
            Assert.All(items, item => Assert.Contains(item, ids));
            Assert.True(sut.LoadingTask.IsCompleted);
            news.Verify(n => n.GetStoryListAsync(It.IsAny<StoryKind>()), Times.Once);
        }

        [Theory]
        [InlineData(StoryKind.Top)] 
        [InlineData(StoryKind.Best)]
        [InlineData(StoryKind.New)]
        public async Task FetchTheRightKindOfStories(StoryKind storyKind)
        {
            var items = Enumerable.Range(0, 20).ToList();
            var news = News;
            var actualStoryKind = StoryKind.Best;
            news.Setup(n => n.GetStoryListAsync(It.IsAny<StoryKind>()))
                .Callback((StoryKind sk) => actualStoryKind = sk)
                .ReturnsAsync(items)
                .Verifiable();

            var sut = new MockNewsViewModel(Navigation.Object, Messenger.Object, news.Object, Reachability.Object, Features.Object, Browser.Object, Dialog.Object);
            sut.SetStoryKind(storyKind);

            await sut.Initialize();
            sut.ViewCreated();

            Assert.Equal(storyKind, actualStoryKind);
            news.Verify(n => n.GetStoryListAsync(storyKind), Times.Once);
        }

        [Fact]
        public async Task FillItemsAsTheyCome()
        {
            Action<NewsItemMessage> callback = null;
            var messenger = new Mock<IMvxMessenger>();
            messenger.Setup(m => m.Subscribe(It.IsAny<Action<NewsItemMessage>>(), It.IsAny<MvxReference>(), It.IsAny<string>()))
                .Callback((Action<NewsItemMessage> action, MvxReference mvxref, string tag) => callback = action)
                .Returns(new MvxSubscriptionToken(Guid.NewGuid(), () => { }))
                .Verifiable();

            var items = Enumerable.Range(0, 20).ToList();
            var news = News;
            news.Setup(n => n.GetStoryListAsync(It.IsAny<StoryKind>()))
                .ReturnsAsync(items)
                .Verifiable();

            var sut = new MockNewsViewModel(Navigation.Object, messenger.Object, news.Object, Reachability.Object, Features.Object, Browser.Object, Dialog.Object);

            await sut.Initialize();
            sut.ViewCreated();

            messenger.Verify(m => m.Subscribe(It.IsAny<Action<NewsItemMessage>>(), It.IsAny<MvxReference>(), It.IsAny<string>()), Times.Once);
            news.Verify(n => n.GetStoryListAsync(It.IsAny<StoryKind>()), Times.Once);
            Assert.True(sut.LoadingTask.IsCompleted);
            Assert.NotNull(callback);

            foreach (var item in items)
            {
                var story = new Item {Id = item, Title = $"Item {item}"};
                callback(new NewsItemMessage(news.Object, story));
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

            Action<NewsItemMessage> callback = null;
            var messenger = new Mock<IMvxMessenger>();
            messenger.Setup(m => m.Subscribe(It.IsAny<Action<NewsItemMessage>>(), It.IsAny<MvxReference>(), It.IsAny<string>()))
                .Callback((Action<NewsItemMessage> action, MvxReference mvxref, string tag) => callback = action)
                .Returns(new MvxSubscriptionToken(Guid.NewGuid(), () => { }));

            var sut = new MockNewsViewModel(navigation.Object, messenger.Object, news.Object, Reachability.Object, Features.Object, Browser.Object, Dialog.Object);

            await sut.Initialize();
            sut.ViewCreated();

            callback(new NewsItemMessage(news.Object, item));

            sut.ShowStoryCommand.TryExecute(sut.Stories[0]);

            Assert.False(sut.ShowStoryCommand.CanExecute(sut.Stories[0]));
            navigation.Verify(n => n.Navigate<StoryViewModel, IStory>(It.IsAny<IStory>(), It.IsAny<MvxBundle>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task TappingAnItemShowsItInCustomBrowser()
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

            Action<NewsItemMessage> callback = null;
            var messenger = new Mock<IMvxMessenger>();
            messenger.Setup(m => m.Subscribe(It.IsAny<Action<NewsItemMessage>>(), It.IsAny<MvxReference>(), It.IsAny<string>()))
                .Callback((Action<NewsItemMessage> action, MvxReference mvxref, string tag) => callback = action)
                .Returns(new MvxSubscriptionToken(Guid.NewGuid(), () => { }));

            var features = Features;
            features.Setup(f => f.IsEnabled(F.OpenStoryInCustomBrowser))
                .Returns(true);

            var sut = new MockNewsViewModel(navigation.Object, messenger.Object, news.Object, Reachability.Object, features.Object, Browser.Object, Dialog.Object);

            await sut.Initialize();
            sut.ViewCreated();

            callback(new NewsItemMessage(news.Object, item));

            sut.ShowStoryCommand.TryExecute(sut.Stories[0]);

            navigation.Verify();
            Assert.Same(item, paramItem);
        }

        [Fact]
        public async Task TappingAnItemShowsItInExternalBrowser()
        {
            var item = new Item 
            {
                Id = 99,
                Type = ItemType.Story,
                Url = "https://kipters.net"
            };

            var navigation = Navigation;
            navigation.Setup(n => n.Navigate<StoryViewModel, IStory>(item, It.IsAny<IMvxBundle>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var news = News;
            news.Setup(n => n.GetStoryListAsync(It.IsAny<StoryKind>()))
                .ReturnsAsync(new List<int> {item.Id});

            Action<NewsItemMessage> callback = null;
            var messenger = new Mock<IMvxMessenger>();
            messenger.Setup(m => m.Subscribe(It.IsAny<Action<NewsItemMessage>>(), It.IsAny<MvxReference>(), It.IsAny<string>()))
                .Callback((Action<NewsItemMessage> action, MvxReference mvxref, string tag) => callback = action)
                .Returns(new MvxSubscriptionToken(Guid.NewGuid(), () => { }));

            var features = Features;
            features.Setup(f => f.IsEnabled(F.OpenStoryInCustomBrowser))
                .Returns(false);

            var browser = Browser;
            string actualUrl = null;
            browser.Setup(b => b.ShowInBrowserAsync(It.IsAny<Uri>(), true))
                .Callback((Uri u, bool _) => actualUrl = u.OriginalString)
                .ReturnsAsync(true)
                .Verifiable();

            var sut = new MockNewsViewModel(navigation.Object, messenger.Object, news.Object, Reachability.Object, features.Object, Browser.Object, Dialog.Object);

            await sut.Initialize();
            sut.ViewCreated();

            callback(new NewsItemMessage(news.Object, item));

            sut.ShowStoryCommand.TryExecute(sut.Stories[0]);

            browser.Verify();
            Assert.Same(item.Url, actualUrl);
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

            var sut = new MockNewsViewModel(navigation.Object, Messenger.Object, news.Object, Reachability.Object, Features.Object, Browser.Object, Dialog.Object);

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

            var sut = new MockNewsViewModel(Navigation.Object, Messenger.Object, news.Object, Reachability.Object, Features.Object, Browser.Object, Dialog.Object);

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

        [Fact]
        public void ShowSettingsCommandNavigatesToSettings()
        {
            var navigation = Navigation;
            navigation
                .Setup(n => n.Navigate<SettingsViewModel>(null, default))
                .ReturnsAsync(true)
                .Verifiable();

            var sut = new MockNewsViewModel(navigation.Object, Messenger.Object, News.Object, Reachability.Object, Features.Object, Browser.Object, Dialog.Object);

            sut.ShowSettingsCommand.TryExecute();

            navigation.Verify();
        }
    }
}