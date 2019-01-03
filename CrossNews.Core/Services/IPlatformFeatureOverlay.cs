using System.Collections.Generic;

namespace CrossNews.Core.Services
{
    public interface IPlatformFeatureOverlay
    {
        IReadOnlyDictionary<string, bool> Overrides { get; }
    }
}
