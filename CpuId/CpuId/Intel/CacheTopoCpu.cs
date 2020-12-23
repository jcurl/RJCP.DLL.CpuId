namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;

    /// <summary>
    /// Describes a CPU cache descriptor used for instruction or data.
    /// </summary>
    public class CacheTopoCpu : CacheTopo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheTopoCpu"/> class.
        /// </summary>
        /// <param name="level">The cache level.</param>
        /// <param name="cacheType">Type of the cache.</param>
        /// <param name="ways">
        /// The ways or slots (associativity) for each set in the cache. This can be zero (0) to indicate fully
        /// associative.
        /// </param>
        /// <param name="lineSize">Size of the line in bytes.</param>
        /// <param name="sizekb">The total size of the cache, in kilobytes.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ways"/> for associativity ways must be zero (0) for fully associative, or positive.
        /// <para>- or -</para>
        /// The <paramref name="lineSize"/> must be positive.
        /// <para>- or -</para>
        /// <paramref name="sizekb"/> must be positive.
        /// </exception>
        /// <remarks>
        /// A cache is organized by bytes per line ( <paramref name="lineSize"/>), then the associativity as the number
        /// of <paramref name="ways"/> per set, with a total size of <paramref name="sizekb"/> kilobytes. This
        /// constructor assumes one partition.
        /// </remarks>
        public CacheTopoCpu(int level, CacheType cacheType, int ways, int lineSize, int sizekb)
            : base(level, cacheType)
        {
            CheckCacheType(cacheType);
            if (ways < 0)
                throw new ArgumentOutOfRangeException(nameof(ways), "Associativity ways must zero (0) for fully associative, or be positive");
            if (lineSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(lineSize), "LineSizes must be positive");
            if (sizekb <= 0)
                throw new ArgumentOutOfRangeException(nameof(sizekb), "Size must be positive");

            Partitions = 1;
            Size = sizekb * 1024;
            LineSize = lineSize;
            if (ways == 0) {
                Associativity = Size / LineSize;
                Sets = 1;
            } else {
                Associativity = ways;
                Sets = Size / LineSize / ways;
            }

            // This mask is undefined.
            Mask = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheTopoCpu"/> class.
        /// </summary>
        /// <param name="level">The cache level.</param>
        /// <param name="cacheType">Type of the cache.</param>
        /// <param name="ways">The ways or slots (associativity) for each set in the cache.</param>
        /// <param name="lineSize">Size of the line in bytes.</param>
        /// <param name="sets">The number of sets.</param>
        /// <param name="partitions">The partitions.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ways"/> for associativity ways must be positive.
        /// <para>- or -</para>
        /// The <paramref name="lineSize"/> must be positive.
        /// <para>- or -</para>
        /// <paramref name="sets"/> must be positive.
        /// <para>- or -</para>
        /// <paramref name="partitions"/> must be positive.
        /// </exception>
        public CacheTopoCpu(int level, CacheType cacheType, int ways, int lineSize, int sets, int partitions)
            : base(level, cacheType)
        {
            CheckCacheType(cacheType);
            if (ways <= 0)
                throw new ArgumentOutOfRangeException(nameof(ways), "Associativity ways must be positive");
            if (lineSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(lineSize), "LineSize must be positive");
            if (sets <= 0)
                throw new ArgumentOutOfRangeException(nameof(sets), "Sets must be positive");
            if (partitions <= 0)
                throw new ArgumentOutOfRangeException(nameof(partitions), "Partitions must be positive");

            Associativity = ways;
            LineSize = lineSize;
            Sets = sets;
            Partitions = 1;
            Size = LineSize * ways * sets * partitions;

            // This mask is undefined.
            Mask = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheTopoCpu"/> class.
        /// </summary>
        /// <param name="level">The cache level.</param>
        /// <param name="cacheType">Type of the cache.</param>
        /// <param name="ways">The ways or slots (associativity) for each set in the cache.</param>
        /// <param name="lineSize">Size of the line in bytes.</param>
        /// <param name="sets">The number of sets.</param>
        /// <param name="partitions">The partitions.</param>
        /// <param name="mask">
        /// Sets the APIC mask for cores that share this cache. 0 indicates that no core shares this mask, -1 indicates
        /// undefined.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ways"/> for associativity ways must be positive.
        /// <para>- or -</para>
        /// The <paramref name="lineSize"/> must be positive.
        /// <para>- or -</para>
        /// <paramref name="sets"/> must be positive.
        /// <para>- or -</para>
        /// <paramref name="partitions"/> must be positive.
        /// </exception>
        public CacheTopoCpu(int level, CacheType cacheType, int ways, int lineSize, int sets, int partitions, long mask)
            : base(level, cacheType)
        {
            CheckCacheType(cacheType);
            if (ways <= 0)
                throw new ArgumentOutOfRangeException(nameof(ways), "Associativity ways must be positive");
            if (lineSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(lineSize), "LineSize must be positive");
            if (sets <= 0)
                throw new ArgumentOutOfRangeException(nameof(sets), "Sets must be positive");
            if (partitions <= 0)
                throw new ArgumentOutOfRangeException(nameof(partitions), "Partitions must be positive");

            Associativity = ways;
            LineSize = lineSize;
            Sets = sets;
            Partitions = 1;
            Size = LineSize * ways * sets * partitions;

            Mask = mask;
        }

        private void CheckCacheType(CacheType cacheType)
        {
            if (((int)cacheType & (int)CacheType.TypeMaskKind) != 0) {
                throw new ArgumentException("CacheType not valid for this structure", nameof(cacheType));
            }
        }

        /// <summary>
        /// The ways of associativity.
        /// </summary>
        /// <value>The ways of associativity.</value>
        /// <remarks>
        /// An associativity of 1 is direct mapped. Other values are the number of entries / slots for each set in the
        /// cache.
        /// </remarks>
        public int Associativity { get; private set; }

        /// <summary>
        /// Gets the size of each line, in bytes.
        /// </summary>
        /// <value>The size of each line, in bytes.</value>
        public int LineSize { get; private set; }

        /// <summary>
        /// Gets the number of cache partitions.
        /// </summary>
        /// <value>The number of cache partitions.</value>
        public int Partitions { get; private set; }

        /// <summary>
        /// Gets the number of sets in the cache.
        /// </summary>
        /// <value>The number of sets. If there is only one set, the cache is fully associative.</value>
        public int Sets { get; private set; }

        /// <summary>
        /// Gets the size of the cache in bytes.
        /// </summary>
        /// <value>The size of the cache, in bytes.</value>
        public int Size { get; private set; }

        /// <summary>
        /// Defines the APIC mask for which cores share this cache.
        /// </summary>
        /// <value>The share mask against the APIC identifier.</value>
        /// <remarks>
        /// A value of 0, indicates that this is not shared with any other core. A value of -1, indicates that this is
        /// undefined. The value returned is a bitmask, where the bits that are set indicate the bits in the APIC Id
        /// that share this mask. To find all the cores that share this cache, take the inverse and mask with a bit-wise
        /// AND of the APIC identifier. Those with the same value after the mask share this cache.
        /// <para>
        /// This method is protected to allow other implementations to set the value after instantiation if required.
        /// But user code should not be able to change this value after it is set.
        /// </para>
        /// </remarks>
        public long Mask { get; internal protected set; }
    }
}
