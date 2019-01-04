using System.Collections.Generic;
using CrossNews.Core.Services;
using MvvmCross.ViewModels;

namespace CrossNews.Core.ViewModels
{
    public class FeatureTogglesViewModel : MvxViewModel
    {
        public FeatureTogglesViewModel(IFeatureStore features) => Toggles = features.Toggles;

        public IReadOnlyDictionary<string, bool> Toggles { get; }
    }
}
