using System;
using System.Threading;

namespace Aii.Assurance.Common
{
    /// <summary>
    /// From Pro Async w/ dotNET by Blewet and Clymer
    /// // provides an convenient way to wrape a resource for async usage
    /// </summary>
    public static class LockExtensions
    {
        public struct LockHelper : IDisposable
        {
            private readonly object obj;
            public LockHelper(object obj)
            {
                this.obj = obj;
            }

            public void Dispose()
            {
                Monitor.Exit(obj);
            }
        }

        public static LockHelper Lock(this object obj, TimeSpan timeout)
        {
            bool lock_taken = false;

            try
            {
                Monitor.TryEnter(obj, TimeSpan.FromSeconds(30), ref lock_taken);
                if (lock_taken)
                {
                    return new LockHelper(obj);
                } else {
                    throw new TimeoutException("Failed to acquire state guard");
                }

            }
            catch
            {
                if (lock_taken)
                {
                    Monitor.Exit(obj);
                }
                throw;
            }
        }

        
    }
}
