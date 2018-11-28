namespace CrossNews.Core.Services
{
    public interface IFeatureStore
    {
        bool IsEnabled(string key);
    }
}
