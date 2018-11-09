using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CrossNews.Core.Model.Api;
using CrossNews.Core.Services;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace CrossNews.Core.ViewModels
{
    public class StoryViewModel : MvxViewModel<IStory>
    {
        private readonly IShareService _share;
        private readonly IBrowserService _browser;
        private readonly IDialogService _dialog;

        private IStory _story;

        public StoryViewModel(IShareService share, IBrowserService browser, IDialogService dialog)
        {
            _share = share;
            _browser = browser;
            _dialog = dialog;

            ShareStoryCommand = new MvxAsyncCommand(
                () => _share.ShareLinkAsync(Title, StoryUrl));
            ShareCommentsCommand = new MvxAsyncCommand(
                () => _share.ShareLinkAsync(Title, $"https://news.ycombinator.com/item?id={_story.Id}"));
            OpenInExternalBrowserCommand = new MvxAsyncCommand(
                () => _browser.ShowInBrowserAsync(new Uri(StoryUrl), false));
        }

        public override void Prepare(IStory story)
        {
            _story = story;
        }

        public string Title => _story.Title;
        public string StoryUrl => _story.Url;
        public int Score => _story.Score;
        public string Author => _story.By;
        public int CommentsCount => _story.Descendants;

        public ICommand ShareStoryCommand { get; }
        public ICommand ShareCommentsCommand { get; }
        public ICommand OpenInExternalBrowserCommand { get; }
        
        public async Task HandleExternalLinkAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url) 
                || !Uri.TryCreate(url, UriKind.Absolute, out var uri))
                return;

            var title = "External link";
            var msg = $"Do you want to leave the app to visit {uri.Host}?";

            var confirmed = await _dialog.ConfirmAsync(title, msg, "Yes", "No");

            if (confirmed)
                await _browser.ShowInBrowserAsync(uri, true);
        }
    }
}
