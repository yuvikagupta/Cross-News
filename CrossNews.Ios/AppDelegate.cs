using CrossNews.Core;
using Foundation;
using MvvmCross.Platforms.Ios.Core;
using UIKit;

namespace CrossNews.Ios
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
    }
}

