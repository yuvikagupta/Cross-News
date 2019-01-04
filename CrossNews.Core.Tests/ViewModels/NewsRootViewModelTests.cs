using CrossNews.Core.Extensions;
using CrossNews.Core.ViewModels;
using Moq;
using MvvmCross.Navigation;
using Xunit;

namespace CrossNews.Core.Tests.ViewModels
{
    public class NewsRootViewModelTests : ViewModelFixtureBase
    {
        private Mock<IMvxNavigationService> Navigation => new Mock<IMvxNavigationService>();

        [Fact]
        public void ShowSettingsCommandNavigatesToSettings()
        {
            var navigation = Navigation;
            navigation
                .Setup(n => n.Navigate<SettingsViewModel>(null, default))
                .ReturnsAsync(true)
                .Verifiable();

            var sut = new NewsRootViewModel(navigation.Object);

            sut.ShowSettingsCommand.TryExecute();

            navigation.Verify();
        }
    }
}