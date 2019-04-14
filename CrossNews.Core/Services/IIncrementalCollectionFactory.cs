using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrossNews.Core.Services
{
    public interface IIncrementalCollectionFactory
    {
        IIncrementalCollection<T> Create<T>(Func<int, Task<IList<T>>> loadAction);
    }
}