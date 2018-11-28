using System.Collections.Generic;
using static CrossNews.Core.Services.Features;

namespace CrossNews.Core.Services
{
    public class FeatureStoreService : IFeatureStore
    {
        private readonly Dictionary<string, bool> _store;

        public FeatureStoreService(IPlatformFeatureOverrides platformOverrides)
        {
            _store = new Dictionary<string, bool>
            {
            };

            foreach (var pair in platformOverrides.Overrides)
            {
                _store[pair.Key] = pair.Value;
            }
        }

        public bool IsEnabled(string key) =>
            _store.TryGetValue(key, out var value) && value;
    }

    public static class Features
    {
    }
}
