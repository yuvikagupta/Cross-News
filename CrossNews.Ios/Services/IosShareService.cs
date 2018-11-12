using System;
using System.Threading.Tasks;
using CoreGraphics;
using CrossNews.Core.Services;
using Foundation;
using UIKit;

namespace CrossNews.Ios.Services
{
    public class IosShareService : IShareService
    {
        public async Task<bool> ShareLinkAsync(string title, string url, object opaqueSource)
        {
            var nsUrl = new NSUrl(url);
            var nsTitle = new NSString(title);
            var vc = new UIActivityViewController(new NSObject[] { nsTitle, nsUrl }, null)
            {
                ExcludedActivityTypes = new NSString[0]
            };

            var popover = vc.PopoverPresentationController;
            if (popover != null)
            {
                switch (opaqueSource)
                {
                    case UIBarButtonItem barButton:
                        popover.BarButtonItem = barButton;
                        break;
                    case UIView view:
                        popover.SourceView = view;
                        break;
                    case CGRect rect:
                        popover.SourceRect = rect;
                        break;
                    default:
                        throw new ArgumentException("Must be a UIBarButtonItem, UIView or CGRect instance", nameof(opaqueSource));
                }
            }

            var root = UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (root is UINavigationController nav)
                await nav.TopViewController.PresentViewControllerAsync(vc, true);
            else
                await root.PresentViewControllerAsync(vc, true);

            return true;
        }
    }
}
