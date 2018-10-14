using System.Collections.Generic;

namespace CrossNews.Core.Model.Api
{
    public interface IComment : IItem
    {
        List<int> Kids { get; }
        int Parent { get; }
        string Text { get; }
    }
}