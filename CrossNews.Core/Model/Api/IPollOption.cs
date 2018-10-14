namespace CrossNews.Core.Model.Api
{
    public interface IPollOption : IItem
    {
        int Poll { get; }
        int Score { get; }
        string Text { get; }
    }
}