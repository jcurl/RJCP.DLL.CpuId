namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;

    /// <summary>
    /// The CPU trace cache.
    /// </summary>
    public class CacheTopoTrace : CacheTopo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheTopoTrace"/> class.
        /// </summary>
        /// <param name="cacheType">Type of the cache.</param>
        /// <param name="ways">The number of associativity ways.</param>
        /// <param name="size">The size of the cache in micro-ops.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Associativity <paramref name="ways"/> must be positive.
        /// <para>- or -</para>
        /// <paramref name="size"/> must be positive.
        /// </exception>
        public CacheTopoTrace(CacheType cacheType, int ways, int size)
            : base(1, cacheType)
        {
            CheckCacheType(cacheType);
            if (ways <= 0)
                throw new ArgumentOutOfRangeException(nameof(ways), "Associativity ways must be positive");
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be positive");

            Associativity = ways;
            Size = size;
        }

        private void CheckCacheType(CacheType cacheType)
        {
            if (((int)cacheType & (int)CacheType.TypeMaskKind) != (int)CacheType.Trace) {
                throw new ArgumentException("CacheType not valid for this structure", nameof(cacheType));
            }
        }

        /// <summary>
        /// Gets the associativity of the cache.
        /// </summary>
        /// <value>The associativity of the cache.</value>
        public int Associativity { get; private set; }

        /// <summary>
        /// Gets the size of the cache in micro-ops.
        /// </summary>
        /// <value>The size of the cache, in micro ops.</value>
        public int Size { get; private set; }
    }
}
