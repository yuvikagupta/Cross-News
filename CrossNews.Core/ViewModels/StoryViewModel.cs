using CrossNews.Core.Model.Api;
using MvvmCross.ViewModels;

namespace CrossNews.Core.ViewModels
{
    public class StoryViewModel : MvxViewModel<IStory>
    {
        private IStory _story;

        public override void Prepare(IStory story)
        {
            _story = story;
        }
    }
}