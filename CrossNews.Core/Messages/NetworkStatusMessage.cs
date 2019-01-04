using MvvmCross.Plugin.Messenger;

namespace CrossNews.Core.Messages
{
    public class NetworkStatusMessage : MvxMessage
    {
        public NetworkStatus Status { get; }

        public NetworkStatusMessage(object sender, NetworkStatus status) : base(sender) => Status = status;
    }

    public enum NetworkStatus
    {
        Unknown,
        Connected,
        Reconnecting,
        Disconnected,
    }
}
