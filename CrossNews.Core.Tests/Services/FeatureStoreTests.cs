using System.Collections.Generic;
using CrossNews.Core.Services;
using Moq;
using Xunit;

namespace CrossNews.Core.Tests.Services
{
    public class FeatureStoreTests
    {
        private readonly Mock<IFeatureProvider> Base = new Mock<IFeatureProvider>();
        private readonly Mock<IPlatformFeatureOverlay> Overlay = new Mock<IPlatformFeatureOverlay>();
        private Mock<IPlatformFeatureOverlay> DummyOverlay 
        {
            get
            {
                var overlay = Overlay;
                overlay.SetupGet(o => o.Overrides)
                    .Returns(new Dictionary<string, bool>());
                return overlay;
            }
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void PlatformOverlayReplacesDefaultValue(bool baseValue, bool overlayValue)
        {
            var baseProvider = Base;
            baseProvider.SetupGet(p => p.Features)
                .Returns(new Dictionary<string, bool>{ ["Foo"] = baseValue });

            var overlay = Overlay;
            overlay.SetupGet(p => p.Overrides)
                .Returns(new Dictionary<string, bool>{ ["Foo"] = overlayValue });

            var sut = new FeatureStoreService(baseProvider.Object, overlay.Object);

            Assert.Equal(overlayValue, sut.IsEnabled("Foo"));
        }

        [Fact]
        public void CanQueryNonExistingFeatures()
        {
            var baseProvider = Base;
            baseProvider.SetupGet(p => p.Features)
                .Returns(new Dictionary<string, bool>());

            var sut = new FeatureStoreService(baseProvider.Object, DummyOverlay.Object);

            var ex = Record.Exception(() => sut.IsEnabled("FakeFeature"));

            Assert.Null(ex);
        }

        [Fact]
        public void NonExistingFeaturesAreDisabledByDefault()
        {
            var baseProvider = Base;
            baseProvider.SetupGet(p => p.Features)
                .Returns(new Dictionary<string, bool>());

            var sut = new FeatureStoreService(baseProvider.Object, DummyOverlay.Object);

            Assert.False(sut.IsEnabled("FakeFeature"));
        }
    }
}
