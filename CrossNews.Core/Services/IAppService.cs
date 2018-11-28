namespace CrossNews.Core.Services
{
    public interface IAppService
    {
        string Version { get; }
        int BuildNumber { get; }
        string Name { get; }
        string Platform { get; }
    }
}
