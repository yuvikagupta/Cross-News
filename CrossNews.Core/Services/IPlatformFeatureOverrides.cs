using System.Collections.Generic;

namespace CrossNews.Core.Services
{
    public interface IPlatformFeatureOverrides
    {
        IReadOnlyDictionary<string, bool> Overrides { get; }
    }
}
