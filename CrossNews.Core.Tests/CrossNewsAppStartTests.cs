using System.Threading.Tasks;
using CrossNews.Core.Services;
using CrossNews.Core.Tests.ViewModels;
using CrossNews.Core.ViewModels;
using Moq;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Xunit;

namespace CrossNews.Core.Tests
{
    public class CrossNewsAppStartTests : ViewModelFixtureBase
    {
        private Mock<IMvxNavigationService> Navigation => new Mock<IMvxNavigationService>();
        private Mock<IMvxApplication> Application => new Mock<IMvxApplication>();
        private Mock<IFeatureStore> FeatureStore => new Mock<IFeatureStore>();

        [Fact]
        public async Task NavigateToTopNewsIfTabsAreDisabledAsync()
        {
            var navigation = Navigation;
            navigation.Setup(n => n.Navigate<TopNewsViewModel>(null, default))
                .ReturnsAsync(true)
                .Verifiable();

            var featureStore = FeatureStore;
            featureStore.Setup(f => f.IsEnabled(Features.StoryTabPresentation))
                .Returns(false)
                .Verifiable();

            var sut = new CrossNewsAppStart(Application.Object, navigation.Object, featureStore.Object);
            await sut.StartAsync();

            navigation.Verify(n => n.Navigate<TopNewsViewModel>(null, default), Times.Once);
            featureStore.Verify(f => f.IsEnabled(Features.StoryTabPresentation));
        }

        [Fact]
        public async Task NavigateToNewsRootIfTabsAreEnabledAsync()
        {
            var navigation = Navigation;
            navigation.Setup(n => n.Navigate<NewsRootViewModel>(null, default))
                .ReturnsAsync(true)
                .Verifiable();

            var featureStore = FeatureStore;
            featureStore.Setup(f => f.IsEnabled(Features.StoryTabPresentation))
                .Returns(true)
                .Verifiable();

            var sut = new CrossNewsAppStart(Application.Object, navigation.Object, featureStore.Object);
            await sut.StartAsync();

            navigation.Verify(n => n.Navigate<NewsRootViewModel>(null, default), Times.Once);
            featureStore.Verify(f => f.IsEnabled(Features.StoryTabPresentation));
        }
    }
}
