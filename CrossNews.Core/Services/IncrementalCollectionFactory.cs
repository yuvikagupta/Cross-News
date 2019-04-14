using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrossNews.Core.Services
{
    internal class IncrementalCollectionFactory : IIncrementalCollectionFactory
    {
        public IIncrementalCollection<T> Create<T>(Func<int, Task<IList<T>>> loadAction)
        {
            return new IncrementalCollection<T>(loadAction);
        }
    }
}