using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CrossNews.Core.Messages;
using CrossNews.Core.Model.Api;
using CrossNews.Core.Services;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace CrossNews.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigation;
        private readonly IMvxMessenger _messenger;
        private readonly INewsService _news;
        private readonly IDialogService _dialog;
        private readonly MvxSubscriptionToken _fillerToken;
        private readonly MvxSubscriptionToken _debugToken;

        private Dictionary<int, StoryItemViewModel> _storyLookup;


        public MainViewModel(IMvxNavigationService navigation, IMvxMessenger messenger, INewsService news, IDialogService dialog)
        {
            _news = news;
            _navigation = navigation;
            _messenger = messenger;
            _dialog = dialog;
            ShowStoryCommand = new MvxAsyncCommand<StoryItemViewModel>(OnShowStory);
            ReloadCommand = new MvxAsyncCommand(LoadTopStories);

            _fillerToken = messenger.Subscribe<NewsItemMessage<Item>>(OnItemReceived);
            _debugToken = messenger.SubscribeOnMainThread<DebugMessage>(msg => _dialog.AlertAsync("DEBUG", msg.Text));
        }

        private void OnItemReceived(NewsItemMessage<Item> msg)
        {
            var id = msg.Data.Id;

            if (!_storyLookup.TryGetValue(id, out var wrapper))
                return;

            wrapper.Fill(msg.Data);
            _storyLookup.Remove(id);
        }

        private Task OnShowStory(StoryItemViewModel item) => _navigation.Navigate(typeof(StoryViewModel), item.Story);

        public override async Task Initialize()
        {
            await base.Initialize();
            await LoadTopStories();
        }

        private async Task LoadTopStories()
        {
            var ids = await _news.GetStoryListAsync(StoryKind.Top);
            var items = ids.Select((x, i) => new StoryItemViewModel(x, i)).ToList();

            Stories = new ObservableCollection<StoryItemViewModel>(items);
            _storyLookup = items.ToDictionary(i => i.Id);

            _news.EnqueueItems(ids.ToList());
        }

        private ObservableCollection<StoryItemViewModel> _stories;
        public ObservableCollection<StoryItemViewModel> Stories
        {
            get => _stories;
            set => SetProperty(ref _stories, value);
        }

        public ICommand ShowStoryCommand { get; }
        public ICommand ReloadCommand { get; }
    }
}
