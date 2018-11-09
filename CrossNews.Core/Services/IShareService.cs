using System.Threading.Tasks;

namespace CrossNews.Core.Services
{
    public interface IShareService
    {
        Task<bool> ShareLinkAsync(string title, string url);
    }

}