using System.Collections.Generic;

namespace CrossNews.Core.Model.Api
{
    public interface IStory : IItem
    {
        int Descendants { get; }
        List<int> Kids { get; }
        int Score { get; }
        string Title { get; }
    }
}