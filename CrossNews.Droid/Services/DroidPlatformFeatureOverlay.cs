using System.Collections.Generic;
using CrossNews.Core.Services;
using static CrossNews.Core.Services.Features;

namespace CrossNews.Droid.Services
{
    public class DroidPlatformFeatureOverlay : IPlatformFeatureOverlay
    {
        public IReadOnlyDictionary<string, bool> Overrides { get; } = new Dictionary<string, bool>
        {
            [OpenStoryInCustomBrowser] = Always,
            [StoryTabPresentation] = Never
        };
    }
}
