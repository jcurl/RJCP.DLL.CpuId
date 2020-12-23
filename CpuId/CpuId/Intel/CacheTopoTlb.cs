namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;

    /// <summary>
    /// Describes a CPU cache for Translation Lookaside Buffers used by the Memory Management Unit of the CPU.
    /// </summary>
    public class CacheTopoTlb : CacheTopo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheTopoTlb"/> class.
        /// </summary>
        /// <param name="level">The cache level.</param>
        /// <param name="cacheType">Type of the cache.</param>
        /// <param name="ways">The associativity. Zero means fully associative.</param>
        /// <param name="entries">The total number of entries in the cache.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="ways"/> of associativity ways must fully associative (0) or be positive.
        /// <para>- or -</para>
        /// <paramref name="entries"/> in the cache must be positive.
        /// </exception>
        /// <remarks>
        /// The constructor calculates the number of sets in the cache through the associativity and the number of
        /// entries.
        /// </remarks>
        public CacheTopoTlb(int level, CacheType cacheType, int ways, int entries)
            : base(level, cacheType)
        {
            CheckCacheType(cacheType);
            if (ways < 0)
                throw new ArgumentOutOfRangeException(nameof(ways), "Associativity ways must fully (0) or be positive");
            if (entries <= 0)
                throw new ArgumentOutOfRangeException(nameof(entries), "Entries must be positive");

            Entries = entries;
            if (ways == 0) {
                Associativity = Entries;
                Sets = 1;
            } else {
                Associativity = ways;
                Sets = Entries / ways;
            }
        }

        private void CheckCacheType(CacheType cacheType)
        {
            if (((int)cacheType & (int)CacheType.TypeMaskKind) != (int)CacheType.Tlb) {
                throw new ArgumentException("CacheType not valid for this structure", nameof(cacheType));
            }
        }

        /// <summary>
        /// Gets the associativity.
        /// </summary>
        /// <value>The associativity, where 0 is fully associative.</value>
        public int Associativity { get; private set; }

        /// <summary>
        /// Gets the total number of TLB entries.
        /// </summary>
        /// <value>The number of entries.</value>
        /// <remarks>
        /// TLBs don't have a "size", like a CPU data/instruction cache, but a number of entries which map pages from
        /// virtual memory to physical memory.
        /// </remarks>
        public int Entries { get; private set; }

        /// <summary>
        /// Gets the number of sets in the cache.
        /// </summary>
        /// <value>The number of sets. If there is only one set, the cache is fully associative.</value>
        public int Sets { get; private set; }
    }
}
