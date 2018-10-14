namespace CrossNews.Core.Model.Api
{
    public interface IJob : IItem
    {
        int Score { get; }
        string Text { get; }
        string Title { get; }
        string Url { get; }
    }
}