using System.Collections.Generic;
using CrossNews.Core.Services;
using static CrossNews.Core.Services.Features;

namespace CrossNews.Ios.Services
{
    public class IosPlatformFeatureOverlay : IPlatformFeatureOverlay
    {
        public IReadOnlyDictionary<string, bool> Overrides { get; } = new Dictionary<string, bool>
        {
            [OpenStoryInCustomBrowser] = false,
            [StoryTabPresentation] = true,
        };
    }
}
