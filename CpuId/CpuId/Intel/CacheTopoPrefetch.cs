namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;

    /// <summary>
    /// The Prefetch size for the caches.
    /// </summary>
    public class CacheTopoPrefetch : CacheTopo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheTopoPrefetch"/> class.
        /// </summary>
        /// <param name="cacheType">Type of the cache.</param>
        /// <param name="lineSize">Size of the each prefetch in bytes.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Prefetch Line Size <paramref name="lineSize"/> ways must be positive
        /// </exception>
        public CacheTopoPrefetch(CacheType cacheType, int lineSize)
            : base(1, cacheType)
        {
            CheckCacheType(cacheType);
            if (lineSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(lineSize), "Prefetch Line Size ways must be positive");

            LineSize = lineSize;
        }

        private static void CheckCacheType(CacheType cacheType)
        {
            if (((int)cacheType & (int)CacheType.TypeMaskKind) != (int)CacheType.Prefetch) {
                throw new ArgumentException("CacheType not valid for this structure", nameof(cacheType));
            }
        }

        /// <summary>
        /// Gets the size of the prefetch line in bytes.
        /// </summary>
        /// <value>The size of the prefetch line in bytes.</value>
        public int LineSize { get; private set; }
    }
}
