using System.Collections.Generic;

namespace CrossNews.Core.Services
{
    internal class DummyPlatformOverrides : IPlatformFeatureOverrides
    {
        public IReadOnlyDictionary<string, bool> Overrides { get; } = new Dictionary<string, bool>();
    }
}
