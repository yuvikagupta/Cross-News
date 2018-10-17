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
    }
}
