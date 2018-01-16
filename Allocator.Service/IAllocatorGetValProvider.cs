using System;

namespace Allocator.Service
{
    public interface IAllocatorGetValProvider : IDisposable
    {
        /// <summary>
        /// 取號
        /// </summary>
        /// <returns></returns>
        long GetNextVal(String key);
    }
}