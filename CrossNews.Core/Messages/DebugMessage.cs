using MvvmCross.Plugin.Messenger;

namespace CrossNews.Core.Messages
{
    public class DebugMessage : MvxMessage
    {
        public string Text { get; }

        public DebugMessage(object sender, string text) : base(sender)
        {
            Text = text;
        }
    }
}
