using System;
using CoreGraphics;
using CrossNews.Core.ViewModels;
using Foundation;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;
using WebKit;

namespace CrossNews.Ios.Views
{
    [MvxChildPresentation]
    public class StoryView : MvxViewController<StoryViewModel>
    {
        protected internal StoryView(IntPtr handle) : base(handle) { }

        public StoryView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.White;
        }
    }
}
