using System;

namespace CrossNews.Core.Model.Api
{
    public interface IItem
    {
        int Id { get; }
        ItemType Type { get; }
        DateTime Time { get; }
        string By { get; }
        bool Dead { get; }
    }
}