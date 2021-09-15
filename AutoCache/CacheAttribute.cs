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
    }
}
