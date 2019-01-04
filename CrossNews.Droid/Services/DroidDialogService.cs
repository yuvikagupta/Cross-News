using System;
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

        public DroidDialogService(IMvxAndroidCurrentTopActivity topActivity) => _topActivity = topActivity;

        private Activity TopActivity => _topActivity.Activity;

        public Task AlertAsync(string title, string text, string button)
        {
            var tcs = new TaskCompletionSource<bool>();
            var btnText = button ?? "Ok";

            var alert = new AlertDialog.Builder(TopActivity)
                .SetTitle(title)
                .SetMessage(text)
                .SetPositiveButton(btnText, (sender, args) => tcs.TrySetResult(true))
                .Create();

            void OnDismiss(object sender, EventArgs e)
            {
                alert.DismissEvent -= OnDismiss;
                tcs.TrySetResult(false);
            }

            alert.DismissEvent += OnDismiss;

            alert.Show();

            return tcs.Task;
        }

        public Task<bool> ConfirmAsync(string title, string text, string positiveButton = null, string negativeButton = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            var posBtnText = positiveButton ?? "Ok";
            var negBtnText = negativeButton ?? "Cancel";

            var alert = new AlertDialog.Builder(TopActivity)
               .SetTitle(title)
               .SetMessage(text)
               .SetPositiveButton(posBtnText, (sender, args) => tcs.TrySetResult(true))
               .SetNegativeButton(negBtnText, (sender, args) => tcs.TrySetResult(false))
               .Create();

            void OnDismiss(object sender, EventArgs e)
            {
                alert.DismissEvent -= OnDismiss;
                tcs.TrySetResult(false);
            }

            alert.DismissEvent += OnDismiss;

            alert.Show();

            return tcs.Task;
        }
    }
}