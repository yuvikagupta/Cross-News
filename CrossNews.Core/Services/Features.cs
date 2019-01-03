namespace CrossNews.Core.Services
{
    public static class Features
    {
#if DEBUG
        internal const bool OnlyDebug = true;
        internal const bool OnlyRelease = false;
#else
        internal const bool OnlyDebug = false;
        internal const bool OnlyRelease = true;
#endif

        public const string OpenStoryInCustomBrowser = nameof(OpenStoryInCustomBrowser);
        public const string ShowOverrideUi = nameof(ShowOverrideUi);
    }
}
