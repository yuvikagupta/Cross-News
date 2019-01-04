using Android;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using CrossNews.Core.Messages;
using CrossNews.Core.Services;
using MvvmCross.Plugin.Messenger;

[assembly: UsesPermission(Manifest.Permission.AccessNetworkState)]
namespace CrossNews.Droid.Services
{
    public class DroidReachabilityService : IReachabilityService
    {
        private readonly IMvxMessenger _messenger;
        private readonly ConnectivityManager _manager;

        public DroidReachabilityService(IMvxMessenger messenger)
        {
            _messenger = messenger;
            _manager = (ConnectivityManager) Application.Context.GetSystemService(Context.ConnectivityService);

            if (Build.VERSION.SdkInt <= BuildVersionCodes.N)
            {
                var receiver = new ConnectionBroadcastReceiver(this);
#pragma warning disable CS0618 // Type or member is obsolete
                Application.Context.RegisterReceiver(receiver, new IntentFilter(ConnectivityManager.ConnectivityAction));
#pragma warning restore CS0618 // Type or member is obsolete
            }
            else
            {
                var callback = new ConnectionCallback(this);
                _manager.RegisterDefaultNetworkCallback(callback);
            }
        }

        private NetworkStatus _lastStatus = NetworkStatus.Unknown;
        private NetworkStatus Status
        {
            get => _lastStatus;
            set
            {
                if (value == _lastStatus)
                {
                    return;
                }

                var msg = new NetworkStatusMessage(this, value);
                _messenger.Publish(msg);
                _lastStatus = value;
            }
        }

        public bool IsConnectionAvailable => _manager.ActiveNetworkInfo?.IsConnected ?? false;

        private class ConnectionBroadcastReceiver : BroadcastReceiver
        {
            private readonly DroidReachabilityService _parent;

            public ConnectionBroadcastReceiver(DroidReachabilityService parent)
            {
                _parent = parent;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                NetworkStatus GetStatus()
                {
                    if (intent.HasExtra(ConnectivityManager.ExtraIsFailover))
                    {
                        return NetworkStatus.Connected;
                    }

                    if (intent.HasExtra(ConnectivityManager.ExtraNoConnectivity))
                    {
                        return NetworkStatus.Disconnected;
                    }

#pragma warning disable CS0618 // Type or member is obsolete
                    if (intent.HasExtra(ConnectivityManager.ExtraNetworkInfo))
                    {
                        var info = intent.Extras.Get(ConnectivityManager.ExtraNetworkInfo) as NetworkInfo;
                        return info.IsConnected 
                            ? NetworkStatus.Connected 
                            : NetworkStatus.Reconnecting;
                    }
                    return NetworkStatus.Unknown;
#pragma warning restore CS0618 // Type or member is obsolete
                }

                var status = GetStatus();
                _parent.Status = status;
            }
        }

        private class ConnectionCallback : ConnectivityManager.NetworkCallback
        {
            private readonly DroidReachabilityService _parent;

            public ConnectionCallback(DroidReachabilityService parent) => _parent = parent;

            public override void OnAvailable(Network network) => _parent.Status = NetworkStatus.Connected;
            public override void OnLosing(Network network, int maxMsToLive) => _parent.Status = NetworkStatus.Reconnecting;
            public override void OnLost(Network network) => _parent.Status = NetworkStatus.Reconnecting;
            public override void OnUnavailable() => _parent.Status = NetworkStatus.Disconnected;
        }
    }
}