using Android.App;
using Android.Content.PM;
using CrossNews.Core;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace CrossNews.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/AppTheme")]
    public class SplashScreen : MvxSplashScreenAppCompatActivity<Setup, App>
    {
    }
}