using System.Linq;
using System.Threading.Tasks;
using CrossNews.Core.Services;
using CrossNews.Core.ViewModels;
using Moq;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using Xunit;

namespace CrossNews.Core.Tests.ViewModels
{
    public class MainViewModelTests : ViewModelFixtureBase
    {
        private Mock<IMvxNavigationService> Navigation => new Mock<IMvxNavigationService>();
        private Mock<IMvxMessenger> Messenger => new Mock<IMvxMessenger>();
        private Mock<INewsService> News => new Mock<INewsService>();

        [Fact]
        public async Task LoadsTopStoriesAtStartup()
        {
            var news = News;
            news.Setup(n => n.GetStoryListAsync(StoryKind.Top))
                .ReturnsAsync(Enumerable.Range(1, 300).ToList)
                .Verifiable();

            var sut = new MainViewModel(Navigation.Object, Messenger.Object, news.Object);

            await sut.Initialize();
            sut.ViewCreated();

            news.Verify();
        }
    }
}
