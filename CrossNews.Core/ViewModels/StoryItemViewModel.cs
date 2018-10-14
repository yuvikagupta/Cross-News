using CrossNews.Core.Model.Api;
using MvvmCross.ViewModels;

namespace CrossNews.Core.ViewModels
{
    public class StoryItemViewModel : MvxNotifyPropertyChanged, IFillable<IStory>
    {
        public int Id { get; }
        public int Index { get; }
        public IStory Story { get; private set; }

        public StoryItemViewModel(int id, int index)
        {
            Id = id;
            Index = index;
            Title = $"Item {id}";
        }

        public void Fill(IStory item)
        {
            Story = item;
            Title = item.Title;
            Score = item.Score;
        }

        private int _score;
        public int Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}