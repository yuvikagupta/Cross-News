using System.Threading.Tasks;
using Android.Content;
using CrossNews.Core.Services;
using MvvmCross.Platforms.Android;

namespace CrossNews.Droid.Services
{
    public class DroidShareService : IShareService
    {
        private readonly IMvxAndroidCurrentTopActivity _topActivity;

        public DroidShareService(IMvxAndroidCurrentTopActivity topActivity)
        {
            _topActivity = topActivity;
        }

        public Task<bool> ShareLinkAsync(string title, string url, object opaqueSource)
        {
            var intent = new Intent(Intent.ActionSend)
                .SetType("text/plain")
                .AddFlags(ActivityFlags.ClearWhenTaskReset)
                .PutExtra(Intent.ExtraSubject, title)
                .PutExtra(Intent.ExtraText, url);

            try
            {
                _topActivity.Activity.StartActivity(Intent.CreateChooser(intent, "Share link"));
                return Task.FromResult(true);
            }
            catch (ActivityNotFoundException)
            {
                return Task.FromResult(false);
            }
        }
    }
}
