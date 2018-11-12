using System;
using CoreGraphics;
using CrossNews.Core.Extensions;
using CrossNews.Core.ViewModels;
using CrossNews.Ios.Extensions;
using Foundation;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;
using WebKit;

namespace CrossNews.Ios.Views
{
    [MvxChildPresentation]
    public class StoryView : MvxViewController<StoryViewModel>, IWKUIDelegate, IWKNavigationDelegate
    {
        protected internal StoryView(IntPtr handle) : base(handle) { }

        private UIProgressView _loadBar;
        private WKWebView _webView;

        public StoryView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.White;

            Title = ViewModel.Title;

#pragma warning disable XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Never;
#pragma warning restore XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version

            var shareButton = new UIBarButtonItem(UIBarButtonSystemItem.Action, OnShareButtonClick);
            NavigationItem.RightBarButtonItem = shareButton;

            var webConfig = new WKWebViewConfiguration();
            _webView = new WKWebView(CGRect.Empty, webConfig)
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                NavigationDelegate = this,
                UIDelegate = this
            };
            View.AddSubview(_webView);

            var constraints = new[]
            {
                _webView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor),
                _webView.TopAnchor.ConstraintEqualTo(View.TopAnchor),
                _webView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor),
                _webView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor)
            };

            View.AddConstraints(constraints);

            _loadBar = new UIProgressView(UIProgressViewStyle.Bar)
            {
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            _loadBar.SizeToFit();

            View.AddSubview(_loadBar);

            var guide = this.GetCompatibleLayoutGuide();
            _loadBar.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
            _loadBar.TopAnchor.ConstraintEqualTo(guide.TopAnchor).Active = true;
            _loadBar.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;

            _webView.LoadRequest(new NSUrlRequest(NSUrl.FromString(ViewModel.StoryUrl)));
        }

        [Export("observeValueForKeyPath:ofObject:change:context:")]
        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            if ("estimatedProgress" == keyPath)
            {
                if (_webView.EstimatedProgress == 1.0)
                {
                    _loadBar.Progress = 0;
                    _loadBar.Hidden = true;
                }
                else
                {
                    if (_loadBar.Hidden)
                        _loadBar.Hidden = false;

                    _loadBar.SetProgress((float)_webView.EstimatedProgress, true);
                }
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _webView.AddObserver(this, "estimatedProgress", NSKeyValueObservingOptions.New, IntPtr.Zero);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            _webView.RemoveObserver(this, "estimatedProgress");
        }

        private void OnShareButtonClick(object sender, EventArgs e)
        {
            var alert = UIAlertController.Create("Share", null, UIAlertControllerStyle.ActionSheet);
            var storyAction = UIAlertAction.Create("Story", UIAlertActionStyle.Default, a => ViewModel.ShareStoryCommand.TryExecute(sender));
            var commentsAction = UIAlertAction.Create("Comments", UIAlertActionStyle.Default, a => ViewModel.ShareCommentsCommand.TryExecute(sender));
            var openInSafariAction = UIAlertAction.Create("Open in Safari", UIAlertActionStyle.Default, a => ViewModel.OpenInExternalBrowserCommand.TryExecute());
            var cancelAction = UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null);
            alert.AddAction(storyAction);
            alert.AddAction(commentsAction);
            alert.AddAction(openInSafariAction);
            alert.AddAction(cancelAction);

            var popover = alert.PopoverPresentationController;
            if (popover != null)
                popover.BarButtonItem = (UIBarButtonItem)sender;

            PresentViewController(alert, true, null);
        }
    }
}
