using System.Collections.Generic;

namespace CrossNews.Core.Services
{
    internal interface IFeatureProvider
    {
        IReadOnlyDictionary<string, bool> Features { get; }
    }
}
