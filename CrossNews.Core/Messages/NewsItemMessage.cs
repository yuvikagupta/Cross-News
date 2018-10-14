using MvvmCross.Plugin.Messenger;

namespace CrossNews.Core.Messages
{
    internal class NewsItemMessage<T> : MvxMessage
    {
        public T Data { get; }

        public NewsItemMessage(object sender, T data) : base(sender)
        {
            Data = data;
        }
    }
}
