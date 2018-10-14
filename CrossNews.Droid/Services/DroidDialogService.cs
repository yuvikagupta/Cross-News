using System.Threading.Tasks;
using Android.App;
using CrossNews.Core.Services;
using MvvmCross.Platforms.Android;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace CrossNews.Droid.Services
{
    public class DroidDialogService : IDialogService
    {
        private readonly IMvxAndroidCurrentTopActivity _topActivity;

        public DroidDialogService(IMvxAndroidCurrentTopActivity topActivity)
        {
            _topActivity = topActivity;
        }

        private Activity TopActivity => _topActivity.Activity;

        public Task AlertAsync(string title, string text, string button)
        {
            var tcs = new TaskCompletionSource<bool>();
            var btnText = button ?? "Ok";

            new AlertDialog.Builder(TopActivity)
                .SetTitle(title)
                .SetMessage(text)
                .SetPositiveButton(btnText, (sender, args) => tcs.TrySetResult(true))
                .Create()
                .Show();

            return tcs.Task;
        }
    }
}