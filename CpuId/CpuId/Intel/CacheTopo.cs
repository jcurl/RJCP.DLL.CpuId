namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Text;

    /// <summary>
    /// A cache descriptor.
    /// </summary>
    public abstract class CacheTopo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheTopo"/> class.
        /// </summary>
        /// <param name="cacheLevel">The cache level in the cache hierarchy.</param>
        /// <param name="cacheType">Type of the cache.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="cacheLevel"/> must be 1 or greater.</exception>
        protected CacheTopo(int cacheLevel, CacheType cacheType)
        {
            ThrowHelper.ThrowIfNegativeOrZero(cacheLevel);
            Level = cacheLevel;
            CacheType = cacheType;
        }

        /// <summary>
        /// Gets the cache level.
        /// </summary>
        /// <value>The cache level.</value>
        public int Level { get; private set; }

        /// <summary>
        /// Gets the type of this cache descriptor.
        /// </summary>
        /// <value>The type of this cache descriptor.</value>
        public CacheType CacheType { get; private set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            string cacheType;
            CacheType typeMask = (CacheType)((int)CacheType & (int)CacheType.TypeMask);
            switch (typeMask) {
            case CacheType.Instruction:
                cacheType = "Instruction Cache";
                break;
            case CacheType.Data:
                cacheType = "Data Cache";
                break;
            case CacheType.Unified:
                cacheType = "Unified Cache";
                break;
            case CacheType.Tlb | CacheType.Instruction:
                cacheType = "TLB Instruction Cache";
                break;
            case CacheType.Tlb | CacheType.Data:
                cacheType = "TLB Data Cache";
                break;
            case CacheType.Tlb | CacheType.Unified:
                cacheType = "TLB Shared Cache";
                break;
            case CacheType.Trace:
                cacheType = "Trace Cache";
                break;
            case CacheType.Prefetch:
                cacheType = "Prefetch";
                break;
            case CacheType.StoreOnlyTlb:
                cacheType = "Store Only TLB";
                break;
            case CacheType.LoadOnlyTlb:
                cacheType = "Load Only TLB";
                break;
            default:
                cacheType = "Unknown Cache Type";
                break;
            }

            StringBuilder pageSize = new();
            if (((int)CacheType & (int)CacheType.Page4k) != 0) StringBuilderAppendOr(pageSize, "4k");
            if (((int)CacheType & (int)CacheType.Page2M) != 0) StringBuilderAppendOr(pageSize, "2M");
            if (((int)CacheType & (int)CacheType.Page4M) != 0) StringBuilderAppendOr(pageSize, "4M");
            if (((int)CacheType & (int)CacheType.Page1G) != 0) StringBuilderAppendOr(pageSize, "1G");

            return pageSize.Length == 0
                ? string.Format("L{0} {1}", Level, cacheType)
                : string.Format("L{0} {1} ({2} page size)", Level, cacheType, pageSize.ToString());
        }

        private static void StringBuilderAppendOr(StringBuilder sb, string text)
        {
            if (sb.Length != 0) sb.Append(" or ");
            sb.Append(text);
        }
    }
}
