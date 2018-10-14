using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrossNews.Core.Services
{
    public interface INewsService
    {
        Task<List<int>> GetStoryListAsync(StoryKind kind);
        void EnqueueItems(List<int> ids);
    }

    public enum StoryKind
    {
        Top,
        New,
        Best,
        Ask,
        Show,
        Job
    }
}
