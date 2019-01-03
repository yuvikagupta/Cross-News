using System.Collections.Generic;

namespace CrossNews.Core.Services
{
    internal class DummyPlatformFeatureOverlay : IPlatformFeatureOverlay
    {
        public IReadOnlyDictionary<string, bool> Overrides { get; } = new Dictionary<string, bool>();
    }
}
