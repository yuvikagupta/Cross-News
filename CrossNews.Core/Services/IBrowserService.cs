using System;
using System.Threading.Tasks;

namespace CrossNews.Core.Services
{
    public interface IBrowserService
    {
        Task<bool> ShowInBrowserAsync(Uri uri, bool preferInternal = true);
    }
}
