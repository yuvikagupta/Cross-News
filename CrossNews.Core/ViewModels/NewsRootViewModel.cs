using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CrossNews.Core.ViewModels
{
    public class NewsRootViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigation;

        public NewsRootViewModel(IMvxNavigationService navigation)
        {
            _navigation = navigation;

            ShowInitialViewModelsCommand = new MvxAsyncCommand(OnShowInitialViewModels);
            ShowSettingsCommand = new MvxAsyncCommand(() => _navigation.Navigate<SettingsViewModel>());
        }

        public ICommand ShowInitialViewModelsCommand { get; }
        public ICommand ShowSettingsCommand { get; }

        private Task OnShowInitialViewModels()
        {
            var tasks = new List<Task>
            {
                _navigation.Navigate<TopNewsViewModel>(),
                _navigation.Navigate<RecentNewsViewModel>(),
                _navigation.Navigate<AskNewsViewModel>(),
                _navigation.Navigate<ShowNewsViewModel>(),
                _navigation.Navigate<JobsNewsViewModel>(),
            };

            return Task.WhenAll(tasks);
        }
    }
}
