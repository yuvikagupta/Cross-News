using System;
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
    public class TopNewsViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigation;
        private readonly IMvxMessenger _messenger;
        private readonly INewsService _news;
        private readonly IReachabilityService _reachability;
        private readonly MvxSubscriptionToken _fillerToken;

        private Dictionary<int, StoryItemViewModel> _storyLookup;

        public TopNewsViewModel(IMvxNavigationService navigation
            , IMvxMessenger messenger
            , INewsService news
            , IReachabilityService reachability)
        {
            _navigation = navigation;
            _messenger = messenger;
            _news = news;
            _reachability = reachability;

            _stories = new MvxObservableCollection<StoryItemViewModel>();

            ShowStoryCommand = new MvxAsyncCommand<StoryItemViewModel>(OnShowStory, item => item.Filled);
            RefreshCommand = new MvxAsyncCommand(LoadTopStories);

            _fillerToken = messenger.Subscribe<NewsItemMessage<IStory>>(OnItemReceived);
        }

        private void OnItemReceived(NewsItemMessage<IStory> msg)
        {
            var id = msg.Data.Id;

            if (!_storyLookup.TryGetValue(id, out var wrapper))
                return;

            wrapper.Fill(msg.Data);
            _storyLookup.Remove(id);
        }

        private Task LoadTopStories()
        {
            async Task LoadAsync()
            {
                var ids = await _news.GetStoryListAsync(StoryKind.Top);
                var items = ids.Select((x, i) => new StoryItemViewModel(x, i)).ToList();

                _stories.Clear();
                _stories.AddRange(items);
                _storyLookup = items.ToDictionary(i => i.Id);

                _news.EnqueueItems(ids.ToList());
            }

            var notifyTask = _reachability.IsConnectionAvailable
                ? MvxNotifyTask.Create(LoadAsync)
                : MvxNotifyTask.Create(Task.FromException(new Exception("Connection not available")));

            LoadingTask = notifyTask;
            return notifyTask.Task;
        }

        private Task OnShowStory(StoryItemViewModel item) => _navigation.Navigate<StoryViewModel, IStory>(item.Story);

        public override async void ViewCreated()
        {
            await LoadTopStories();
        }

        private readonly MvxObservableCollection<StoryItemViewModel> _stories;
        public ObservableCollection<StoryItemViewModel> Stories => _stories;

        private MvxNotifyTask _loadingTask;
        public MvxNotifyTask LoadingTask
        {
            get => _loadingTask;
            private set => SetProperty(ref _loadingTask, value);
        }

        public ICommand ShowStoryCommand { get; }

        public ICommand RefreshCommand { get; }
    }
}
