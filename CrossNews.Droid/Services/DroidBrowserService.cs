using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Support.CustomTabs;
using CrossNews.Core.Services;
using MvvmCross.Platforms.Android;

namespace CrossNews.Droid.Services
{
    public class DroidBrowserService : IBrowserService
    {
        private readonly IMvxAndroidCurrentTopActivity _topActivity;

        public DroidBrowserService(IMvxAndroidCurrentTopActivity topActivity) => _topActivity = topActivity;

        public Task<bool> ShowInBrowserAsync(Uri uri, bool preferInternal = true) =>
            preferInternal
                ? LaunchCustomTabActivity(uri)
                : LaunchExternalBrowser(uri);

        private Task<bool> LaunchCustomTabActivity(Uri uri)
        {
            new CustomTabsIntent.Builder()
                .Build()
                .LaunchUrl(_topActivity.Activity, Android.Net.Uri.Parse(uri.ToString()));

            return Task.FromResult(true);
        }

        private Task<bool> LaunchExternalBrowser(Uri uri)
        {
            var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri.ToString()));
            try
            {
                _topActivity.Activity.StartActivity(intent);
                return Task.FromResult(true);
            }
            catch (ActivityNotFoundException)
            {
                return Task.FromResult(false);
            }
        }
    }
}
