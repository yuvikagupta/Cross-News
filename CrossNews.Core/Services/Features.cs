namespace CrossNews.Core.Services
{
    public static class Features
    {
#if DEBUG
        public const bool OnlyDebug = true;
        public const bool OnlyRelease = false;
#else
        public const bool OnlyDebug = false;
        public const bool OnlyRelease = true;
#endif

        public const bool Always = true;
        public const bool Never = false;

        public const string OpenStoryInCustomBrowser = nameof(OpenStoryInCustomBrowser);
        public const string ShowOverrideUi = nameof(ShowOverrideUi);
        public const string StoryTabPresentation = nameof(StoryTabPresentation);
        public const string IncrementalLoading = nameof(IncrementalLoading);
    }
}
