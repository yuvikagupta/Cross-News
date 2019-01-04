using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace CrossNews.Core.Tests
{
    public class MockDispatcher : MvxMainThreadDispatcher, IMvxViewDispatcher
    {
        public readonly List<MvxViewModelRequest> Requests = new List<MvxViewModelRequest>();

        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            action();
            return true;
        }

        public Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true)
        {
            action();
            return Task.CompletedTask;
        }

        public Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true) => action();

        public override bool IsOnMainThread { get; } = true;

        public Task<bool> ShowViewModel(MvxViewModelRequest request)
        {
            Requests.Add(request);
            return Task.FromResult(true);
        }

        public Task<bool> ChangePresentation(MvxPresentationHint hint) => throw new NotImplementedException();
    }
}
