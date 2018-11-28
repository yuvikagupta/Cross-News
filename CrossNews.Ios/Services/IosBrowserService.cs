using System;
using System.Threading.Tasks;
using CrossNews.Core.Services;
using SafariServices;
using UIKit;

namespace CrossNews.Ios.Services
{
    public class IosBrowserService : IBrowserService
    {
        public Task<bool> ShowInBrowserAsync(Uri uri, bool preferInternal = true)
        {
            return preferInternal 
                ? ShowSafariViewController(uri) 
                : LaunchSafari(uri);
        }

        private Task<bool> LaunchSafari(Uri uri)
        {
            var app = UIApplication.SharedApplication;

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
#pragma warning disable XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version
                var options = new UIApplicationOpenUrlOptions();
                return app.OpenUrlAsync(uri, options);
#pragma warning restore XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version
            }

#pragma warning disable XI0003 // Notifies you when using a deprecated, obsolete or unavailable Apple API
            var result = app.OpenUrl(uri);
            return Task.FromResult(result);
#pragma warning restore XI0003 // Notifies you when using a deprecated, obsolete or unavailable Apple API
        }

        private Task<bool> ShowSafariViewController(Uri uri)
        {
            var safari = new SFSafariViewController(uri);
            // Probably a bad idea
            UIApplication.SharedApplication
                .KeyWindow
                .RootViewController
                .PresentViewController(safari, true, null);

            return Task.FromResult(true);
        }
    }
}
