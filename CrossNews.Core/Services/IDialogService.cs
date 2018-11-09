using System.Threading.Tasks;

namespace CrossNews.Core.Services
{
    public interface IDialogService
    {
        Task AlertAsync(string title, string text, string button = null);
        Task<bool> ConfirmAsync(string title, string text, string positiveButton = null, string negativeButton = null);
    }
}
