using Microsoft.Extensions.Caching.Memory;
using System;

namespace AutoCache
{
    /// <summary>
    /// AutoCache configurations for a method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class CacheAttribute : Attribute
    {
        /// <summary>
        /// Duration of cache
        /// </summary>
        public int Seconds { get; set; } = 30;

        /// <summary>
        /// Sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
        /// This will not extend the entry lifetime beyond the absolute expiration (if set).
        /// </summary>
        public int SlidingSeconds { get; set; } = 0;

        /// <summary>
        /// Sets the priority for keeping the cache entry in the cache during a memory pressure triggered cleanup.
        /// The default is Microsoft.Extensions.Caching.Memory.CacheItemPriority.Normal.
        /// </summary>
        public CacheItemPriority Priority { get; set; } = CacheItemPriority.Normal;

    }
}
