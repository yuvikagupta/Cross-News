using System.Collections.Generic;
using static CrossNews.Core.Services.Features;

namespace CrossNews.Core.Services
{
    internal class FeatureProvider : IFeatureProvider
    {
        public FeatureProvider()
        {
            Features = new Dictionary<string, bool>
            {
                [OpenStoryInCustomBrowser] = Always,
                [ShowOverrideUi] = OnlyDebug,
                [StoryTabPresentation] = OnlyDebug,
                [IncrementalLoading] = Never,
            };
        }

        public IReadOnlyDictionary<string, bool> Features { get; }
    }
}
