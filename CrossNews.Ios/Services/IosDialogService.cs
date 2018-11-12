using System;
using System.Threading.Tasks;
using CrossNews.Core.Services;
using UIKit;

namespace CrossNews.Ios.Services
{
    public class IosDialogService : IDialogService
    {
        public Task AlertAsync(string title, string text, string button = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            var btnText = button ?? "Ok";

            var alert = UIAlertController.Create(title, text, UIAlertControllerStyle.Alert);
            var okAction = UIAlertAction.Create(btnText, UIAlertActionStyle.Default, _ => tcs.TrySetResult(true));
            alert.AddAction(okAction);

            UIApplication.SharedApplication
                .KeyWindow
                .RootViewController
                .PresentViewController(alert, true, null);

            return tcs.Task;
        }

        public Task<bool> ConfirmAsync(string title, string text, string positiveButton = null, string negativeButton = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            var posBtnText = positiveButton ?? "Ok";
            var negBtnText = negativeButton ?? "Cancel";

            var alert = UIAlertController.Create(title, text, UIAlertControllerStyle.Alert);
            var okAction = UIAlertAction.Create(posBtnText, UIAlertActionStyle.Default, _ => tcs.TrySetResult(true));
            var cancelAction = UIAlertAction.Create(posBtnText, UIAlertActionStyle.Cancel, _ => tcs.TrySetResult(false));

            alert.AddAction(okAction);

            UIApplication.SharedApplication
                .KeyWindow
                .RootViewController
                .PresentViewController(alert, true, null);

            return tcs.Task;
        }
    }
}
