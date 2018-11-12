using System;
using System.Threading.Tasks;
using CrossNews.Core.Extensions;
using CrossNews.Core.Model.Api;
using CrossNews.Core.Services;
using CrossNews.Core.ViewModels;
using Moq;
using Xunit;

namespace CrossNews.Core.Tests.ViewModels
{
    public class StoryViewModelTests : ViewModelFixtureBase
    {
        private Mock<IShareService> Share => new Mock<IShareService>();
        private Mock<IBrowserService> Browser => new Mock<IBrowserService>();
        private Mock<IDialogService> Dialog = new Mock<IDialogService>();

        [Fact]
        public async Task ShouldShowStoryTitle()
        {
            var sut = new StoryViewModel(Share.Object, Browser.Object, Dialog.Object);
            
            var story = new Mock<IStory>();
            var fakeTitle = "Fake Story Title";
            story.SetupGet(s => s.Title).Returns(fakeTitle);

            sut.Prepare(story.Object);
            await sut.Initialize();

            Assert.Equal(fakeTitle, sut.Title);
        }

        [Fact]
        public async Task ShouldShowStoryUrl()
        {
            var sut = new StoryViewModel(Share.Object, Browser.Object, Dialog.Object);
            
            var story = new Mock<IStory>();
            var fakeUrl = "https://fake-url.com";
            story.SetupGet(s => s.Url).Returns(fakeUrl);

            sut.Prepare(story.Object);
            await sut.Initialize();

            Assert.Equal(fakeUrl, sut.StoryUrl);
        }

        [Fact]
        public async Task ShouldShowStoryScore()
        {
            var sut = new StoryViewModel(Share.Object, Browser.Object, Dialog.Object);
            
            var story = new Mock<IStory>();
            var fakeScore = 99;
            story.SetupGet(s => s.Score).Returns(fakeScore);

            sut.Prepare(story.Object);
            await sut.Initialize();

            Assert.Equal(fakeScore, sut.Score);
        }

        [Fact]
        public async Task ShouldShowStoryAuthor()
        {
            var sut = new StoryViewModel(Share.Object, Browser.Object, Dialog.Object);
            
            var story = new Mock<IStory>();
            var fakeAuthor = "Albert McAuthorFace";
            story.SetupGet(s => s.By).Returns(fakeAuthor);

            sut.Prepare(story.Object);
            await sut.Initialize();

            Assert.Equal(fakeAuthor, sut.Author);
        }

        [Fact]
        public async Task ShouldShowCommentsCount()
        {
            var sut = new StoryViewModel(Share.Object, Browser.Object, Dialog.Object);
            
            var story = new Mock<IStory>();
            var fakeCount = 111;
            story.SetupGet(s => s.Descendants).Returns(fakeCount);

            sut.Prepare(story.Object);
            await sut.Initialize();

            Assert.Equal(fakeCount, sut.CommentsCount);
        }

        [Fact]
        public async Task ShareStoryCommandShouldShareStoryUrl()
        {
            var share = Share;
            var fakeUrl = "https://fake-url.com";
            share.Setup(s => s.ShareLinkAsync(It.IsAny<string>(), fakeUrl, It.IsAny<object>()))
                .ReturnsAsync(true)
                .Verifiable();

            var story = new Mock<IStory>();
            story.SetupGet(s => s.Url).Returns(fakeUrl);

            var sut = new StoryViewModel(share.Object, Browser.Object, Dialog.Object);
            sut.Prepare(story.Object);
            await sut.Initialize();
            
            sut.ShareStoryCommand.TryExecute();

            share.Verify();
        }

        [Fact]
        public async Task ShareStoryCommandShouldShareStoryTitle()
        {
            var share = Share;
            var fakeTitle = "Fake Story Title";
            share.Setup(s => s.ShareLinkAsync(fakeTitle, It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(true)
                .Verifiable();

            var story = new Mock<IStory>();
            story.SetupGet(s => s.Title).Returns(fakeTitle);

            var sut = new StoryViewModel(share.Object, Browser.Object, Dialog.Object);
            sut.Prepare(story.Object);
            await sut.Initialize();
            
            sut.ShareStoryCommand.TryExecute();

            share.Verify();
        }

        [Fact]
        public async Task ShareCommentsCommandShouldShareStoryUrl()
        {
            var share = Share;
            var fakeId = 999;
            var fakeUrl = $"https://news.ycombinator.com/item?id={fakeId}";
            share.Setup(s => s.ShareLinkAsync(It.IsAny<string>(), fakeUrl, It.IsAny<object>()))
                .ReturnsAsync(true)
                .Verifiable();

            var story = new Mock<IStory>();
            story.SetupGet(s => s.Id).Returns(fakeId);

            var sut = new StoryViewModel(share.Object, Browser.Object, Dialog.Object);
            sut.Prepare(story.Object);
            await sut.Initialize();
            
            sut.ShareCommentsCommand.TryExecute();

            share.Verify();
        }

        [Fact]
        public async Task ShareCommentsCommandShouldShareStoryTitle()
        {
            var share = Share;
            var fakeTitle = "Fake Story Title";
            share.Setup(s => s.ShareLinkAsync(fakeTitle, It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(true)
                .Verifiable();

            var story = new Mock<IStory>();
            story.SetupGet(s => s.Title).Returns(fakeTitle);

            var sut = new StoryViewModel(share.Object, Browser.Object, Dialog.Object);
            sut.Prepare(story.Object);
            await sut.Initialize();
            
            sut.ShareStoryCommand.TryExecute();

            share.Verify();
        }

        [Fact]
        public async Task OpenInExternalBrowserCommandShouldOpenStoryUrlInExternalBrowser()
        {
            var browser = Browser;
            var fakeUrl = "https://fake-url.com";
            browser.Setup(b => b.ShowInBrowserAsync(new Uri(fakeUrl), false))
                .ReturnsAsync(true)
                .Verifiable();
            
            var story = new Mock<IStory>();
            story.SetupGet(s => s.Url).Returns(fakeUrl);

            var sut = new StoryViewModel(Share.Object, browser.Object, Dialog.Object);
            sut.Prepare(story.Object);
            await sut.Initialize();

            sut.OpenInExternalBrowserCommand.TryExecute();

            browser.Verify();
        }

        [Fact]
        public async Task ShouldAskUserToOpenExternalLinks()
        {
            var dialog = Dialog;
            string msg = null;
            dialog.Setup(d => d.ConfirmAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string title, string text, string pB, string nB) => msg = text)
                .ReturnsAsync(true)
                .Verifiable();

            var sut = new StoryViewModel(Share.Object, Browser.Object, dialog.Object);
            
            var fakeUrl = "https://kipters.net";
            await sut.HandleExternalLinkAsync(fakeUrl);

            dialog.Verify();
            Assert.True(msg.Contains("kipters.net"));
        }

        [Fact]
        public async Task ShouldOpenExternalLinksInInternalBrowserIfUserAllows()
        {
            var fakeUrl = "https://kipters.net";

            var dialog = Dialog;
            dialog.Setup(d => d.ConfirmAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var browser = Browser;
            browser.Setup(b => b.ShowInBrowserAsync(new Uri(fakeUrl), true))
                .ReturnsAsync(true)
                .Verifiable();

            var sut = new StoryViewModel(Share.Object, browser.Object, dialog.Object);
            
            await sut.HandleExternalLinkAsync(fakeUrl);

            browser.Verify();
        }

        [Fact]
        public async Task ShouldNotOpenExternalLinksIfUserDenies()
        {
            var fakeUrl = "https://kipters.net";

            var dialog = Dialog;
            dialog.Setup(d => d.ConfirmAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var browser = Browser;
            browser.Setup(b => b.ShowInBrowserAsync(It.IsAny<Uri>(), It.IsAny<bool>()))
                .ReturnsAsync(true)
                .Verifiable();

            var sut = new StoryViewModel(Share.Object, Browser.Object, dialog.Object);
            
            await sut.HandleExternalLinkAsync(fakeUrl);

            browser.Verify(b => b.ShowInBrowserAsync(It.IsAny<Uri>(), It.IsAny<bool>()), 
                Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldIgnoreRequestsForNullOrEmptyLinks(string url)
        {
            var dialog = Dialog;
            dialog.Setup(d => d.ConfirmAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true)
                .Verifiable();

            var browser = Browser;
            browser.Setup(b => b.ShowInBrowserAsync(It.IsAny<Uri>(), It.IsAny<bool>()))
                .ReturnsAsync(true)
                .Verifiable();

            var sut = new StoryViewModel(Share.Object, Browser.Object, dialog.Object);
            
            await sut.HandleExternalLinkAsync(url);

            browser.Verify(b => b.ShowInBrowserAsync(It.IsAny<Uri>(), It.IsAny<bool>()), 
                Times.Never);
            dialog.Verify(d => d.ConfirmAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task ShouldIgnoreInvalidUrls()
        {
            var url = "invalidurl";

            var dialog = Dialog;
            dialog.Setup(d => d.ConfirmAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true)
                .Verifiable();

            var browser = Browser;
            browser.Setup(b => b.ShowInBrowserAsync(It.IsAny<Uri>(), It.IsAny<bool>()))
                .ReturnsAsync(true)
                .Verifiable();

            var sut = new StoryViewModel(Share.Object, Browser.Object, dialog.Object);
            
            await sut.HandleExternalLinkAsync(url);

            browser.Verify(b => b.ShowInBrowserAsync(It.IsAny<Uri>(), It.IsAny<bool>()), 
                Times.Never);
            dialog.Verify(d => d.ConfirmAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }
    }
}
