using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public abstract class NewsViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigation;
        private readonly IMvxMessenger _messenger;
        private readonly INewsService _news;
        private readonly IReachabilityService _reachability;
        private readonly IBrowserService _browser;
        private readonly IDialogService _dialog;
        private readonly IIncrementalCollectionFactory _incrementalCollectionFactory;

        private readonly MvxSubscriptionToken _fillerToken;

        private Dictionary<int, StoryItemViewModel> _storyLookup;

        protected abstract StoryKind StoryKind { get; }
        public bool TabPresentation { get; }
        public bool IncrementalLoading { get; }
        private bool CustomBrowser { get; }

        private List<int> _ids;
        private int _idIndex = 0;

        protected NewsViewModel(IMvxNavigationService navigation
            , IMvxMessenger messenger
            , INewsService news
            , IReachabilityService reachability
            , IFeatureStore featureStore
            , IBrowserService browser
            , IDialogService dialog
            , IIncrementalCollectionFactory incrementalCollectionFactory)
        {
            _navigation = navigation;
            _messenger = messenger;
            _news = news;
            _reachability = reachability;
            _browser = browser;
            _dialog = dialog;
            _incrementalCollectionFactory = incrementalCollectionFactory;

            IncrementalLoading = featureStore.IsEnabled(Features.IncrementalLoading);
            CustomBrowser = featureStore.IsEnabled(Features.OpenStoryInCustomBrowser);
            TabPresentation = featureStore.IsEnabled(Features.StoryTabPresentation);

            if (IncrementalLoading)
            {
                var source = _incrementalCollectionFactory.Create(OnIncrementalLoad);
                // On hindsight, having to rely on this sounds like a big hack
                _stories = (MvxObservableCollection<StoryItemViewModel>) source;
            }
            else
            {
                _stories = new MvxObservableCollection<StoryItemViewModel>();
            }

            ShowStoryCommand = new MvxAsyncCommand<StoryItemViewModel>(OnShowStory, item => item.Filled && item.Story.Type == ItemType.Story);
            RefreshCommand = new MvxAsyncCommand(LoadStories);

            ShowSettingsCommand = new MvxAsyncCommand(() => _navigation.Navigate<SettingsViewModel>());

            _fillerToken = messenger.Subscribe<NewsItemMessage>(OnItemReceived);
        }

        private Task<IList<StoryItemViewModel>> OnIncrementalLoad(int count)
        {
            lock (_ids)
            {
                Debug.WriteLine($"Load {GetType().Name} - {_idIndex} - {count}");
                IsBusy = true;
                var ids = _ids
                    .Skip(_idIndex)
                    .Take(count)
                    .ToList();
                
                var items = ids
                    .Select((x, i) => new StoryItemViewModel(x, i + _idIndex))
                    .ToList();

                _stories.AddRange(items);
                foreach (var item in items)
                {
                    _storyLookup[item.Id] = item;
                }

                _news.EnqueueItems(ids);

                _idIndex += count;
                IsBusy = false;
                return Task.FromResult((IList<StoryItemViewModel>) items);
            }
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            if (TabPresentation)
            {
                await LoadStories();
            }
        }

        public override async void ViewCreated()
        {
            base.ViewCreated();

            if (!TabPresentation)
            {
                await LoadStories();
            }
        }

        private void OnItemReceived(NewsItemMessage msg)
        {
            var id = msg.Data.Id;

            if (!_storyLookup.TryGetValue(id, out var wrapper))
            {
                return;
            }

            wrapper.Fill(msg.Data);
            _storyLookup.Remove(id);
        }

        private Task LoadStories()
        {
            async Task LoadAllAsync()
            {
                IsBusy = true;
                _ids = await _news.GetStoryListAsync(StoryKind);
                var items = _ids.Select((x, i) => new StoryItemViewModel(x, i)).ToList();

                _stories.Clear();
                _stories.AddRange(items);
                _storyLookup = items.ToDictionary(i => i.Id);

                _news.EnqueueItems(_ids.ToList());
                IsBusy = false;
            }

            async Task LoadInitialAsync()
            {
                IsBusy = true;
                _ids = await _news.GetStoryListAsync(StoryKind);
                _stories.Clear();
                _storyLookup = new Dictionary<int, StoryItemViewModel>();
                _idIndex = 0;

                await OnIncrementalLoad(30);
            }

            Task LoadAsync() => IncrementalLoading
                    ? LoadInitialAsync()
                    : LoadAllAsync();

            var notifyTask = _reachability.IsConnectionAvailable
                ? MvxNotifyTask.Create(LoadAsync)
                : MvxNotifyTask.Create(_dialog.AlertAsync("Try again", "No internet connection", "OK"));

            LoadingTask = notifyTask;
            return notifyTask.Task;
        }

        private Task OnShowStory(StoryItemViewModel item)
        {
            return CustomBrowser
                ? _navigation.Navigate<StoryViewModel, IStory>(item.Story)
                : _browser.ShowInBrowserAsync(new Uri(item.Story.Url), true);
        }

        private readonly MvxObservableCollection<StoryItemViewModel> _stories;
        public ObservableCollection<StoryItemViewModel> Stories => _stories;

        private MvxNotifyTask _loadingTask;
        public MvxNotifyTask LoadingTask
        {
            get => _loadingTask;
            private set => SetProperty(ref _loadingTask, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            private set => SetProperty(ref _isBusy, value);
        }

        public ICommand ShowStoryCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ShowSettingsCommand { get; }
    }
}
