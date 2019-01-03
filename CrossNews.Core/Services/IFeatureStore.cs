using System.Collections.Generic;

namespace CrossNews.Core.Services
{
    public interface IFeatureStore
    {
        IReadOnlyDictionary<string, bool> Toggles { get; }
        bool IsEnabled(string key);
    }
}
