using System.Collections.Generic;

namespace CrossNews.Core.Model.Api
{
    public interface IPoll : IItem
    {
        int Descendants { get; }
        List<int> Kids { get; }
        List<int> Parts { get; }
        int Score { get; }
        string Text { get; }
        string Title { get; }
    }
}