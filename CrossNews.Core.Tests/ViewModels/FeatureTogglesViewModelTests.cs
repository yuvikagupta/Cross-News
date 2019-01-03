using System.Collections.Generic;
using CrossNews.Core.Services;
using CrossNews.Core.ViewModels;
using Moq;
using Xunit;

namespace CrossNews.Core.Tests.ViewModels
{
    public class FeatureTogglesViewModelTests : ViewModelFixtureBase
    {
        private Mock<IFeatureStore> Features => new Mock<IFeatureStore>();

        [Fact]
        public void ShowsTheTogglesFromTheFeatureStore()
        {
            var toggles = new Dictionary<string, bool>
            {
                ["Foo"] = true,
                ["Bar"] = false
            };

            var features = Features;
            features.SetupGet(f => f.Toggles)
                .Returns(toggles)
                .Verifiable();

            var sut = new FeatureTogglesViewModel(features.Object);

            features.Verify();
            Assert.Equal(toggles, sut.Toggles);
        }
    }
}
