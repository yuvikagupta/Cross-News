using System;
using UIKit;

namespace CrossNews.Ios.Extensions
{
    public static class UIViewControllerExtensions
    {
        public static UILayoutGuide GetCompatibleLayoutGuide(this UIViewController self)
        {
#pragma warning disable XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                return self.View.SafeAreaLayoutGuide;
#pragma warning restore XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version

            var guide = new UILayoutGuide();

#pragma warning disable XI0003 // Notifies you when using a deprecated, obsolete or unavailable Apple API
            guide.LeadingAnchor.ConstraintEqualTo(self.View.LeadingAnchor).Active = true;
            guide.TrailingAnchor.ConstraintEqualTo(self.View.TrailingAnchor).Active = true;
            guide.TopAnchor.ConstraintEqualTo(self.TopLayoutGuide.GetBottomAnchor()).Active = true;
            guide.BottomAnchor.ConstraintEqualTo(self.BottomLayoutGuide.GetTopAnchor()).Active = true;
#pragma warning restore XI0003 // Notifies you when using a deprecated, obsolete or unavailable Apple API

            return guide;
        }
    }
}
