using System.Collections.Generic;
using static CrossNews.Core.Services.Features;

namespace CrossNews.Core.Services
{
    internal class FeatureStoreService : IFeatureStore
    {
        private readonly Dictionary<string, bool> _store;

        public FeatureStoreService(IFeatureProvider baseFeatures, IPlatformFeatureOverlay platformOverlay)
        {
            _store = new Dictionary<string, bool>();

            foreach (var pair in baseFeatures.Features)
            {
                _store[pair.Key] = pair.Value;
            }

            foreach (var pair in platformOverlay.Overrides)
            {
                _store[pair.Key] = pair.Value;
            }

            Toggles = _store;
        }

        public IReadOnlyDictionary<string, bool> Toggles { get; }

        public bool IsEnabled(string key) =>
            _store.TryGetValue(key, out var value) && value;
    }
}
