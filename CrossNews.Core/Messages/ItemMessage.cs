using MvvmCross.Plugin.Messenger;

namespace CrossNews.Core.Messages
{
    internal abstract class ItemMessage<T> : MvxMessage
    {
        public T Data { get; }

        protected ItemMessage(object sender, T data) : base(sender) => Data = data;
    }
}
