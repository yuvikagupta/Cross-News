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
        }

        private bool _filled;
        public bool Filled
        {
            get => _filled;
            private set => SetProperty(ref _filled, value);
        }

        public void Fill(IStory item)
        {
            Story = item;
            Title = item.Title;
            Score = item.Score;
            Author = item.By;
            CommentsCount = item.Descendants;
            Filled = true;
        }

        private int _score;
        public int Score
        {
            get => _score;
            private set => SetProperty(ref _score, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            private set => SetProperty(ref _title, value);
        }

        private string _author;
        public string Author
        {
            get => _author;
            private set => SetProperty(ref _author, value);
        }

        private int _commentsCount;
        public int CommentsCount
        {
            get => _commentsCount;
            private set => SetProperty(ref _commentsCount, value);
        }
    }
}