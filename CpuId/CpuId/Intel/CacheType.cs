namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;

    /// <summary>
    /// The type of cache being described.
    /// </summary>
    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2346:Flags enumerations zero-value members should be named \"None\"",
        Justification = "Invalid name is better descriptive than None")]
    public enum CacheType
    {
        /// <summary>
        /// Cache structure not defined. This valid indicates invalid.
        /// </summary>
        Invalid = 0,

        #region Cache Type: Instruction, Data, TLB, Trace, Prefetch.
        /// <summary>
        /// Defines the mask for the type of cache.
        /// </summary>
        TypeMask = 0x000000FF,

        /// <summary>
        /// Defines the mask for the kind of cache (TLB, CPU, Trace, Prefetch)
        /// </summary>
        TypeMaskKind = 0x000000F0,

        /// <summary>
        /// Instruction cache.
        /// </summary>
        Instruction = 0x00000001,

        /// <summary>
        /// Data cache.
        /// </summary>
        Data = 0x00000002,

        /// <summary>
        /// Unified Cache
        /// </summary>
        Unified = Instruction | Data,

        /// <summary>
        /// Translation Look-aside Buffer cache.
        /// </summary>
        Tlb = 0x00000010,

        /// <summary>
        /// Trace Cache.
        /// </summary>
        Trace = 0x00000020,

        /// <summary>
        /// Prefetch cache.
        /// </summary>
        Prefetch = 0x00000030,
        #endregion

        #region PageSize: 4k, 2M, 4M, 1G
        /// <summary>
        /// Defines the mask for the page type.
        /// </summary>
        PageMask = 0x0000FF00,

        /// <summary>
        /// 4k pages.
        /// </summary>
        Page4k = 0x00000100,

        /// <summary>
        /// 2Mbyte pages.
        /// </summary>
        Page2M = 0x00000200,

        /// <summary>
        /// 4Mbyte pages.
        /// </summary>
        Page4M = 0x00000400,

        /// <summary>
        /// 1Gbyte pages.
        /// </summary>
        Page1G = 0x00000800,
        #endregion

        #region Combined Values from Flags
        /// <summary>
        /// A combination describing TLB instruction cache for 4kb pages.
        /// </summary>
        InstructionTlb4k = Instruction | Tlb | Page4k,

        /// <summary>
        /// A combination describing TLB instruction cache for 4kb and 2Mb or 4Mb pages. The number of entries are often
        /// half for 4Mb pages.
        /// </summary>
        InstructionTlb4k2M4M = Instruction | Tlb | Page4k | Page2M | Page4M,

        /// <summary>
        /// A combination describing TLB instruction cache for 2Mb pages.
        /// </summary>
        InstructionTlb2M = Instruction | Tlb | Page2M,

        /// <summary>
        /// A combination describing TLB instruction cache for 2Mb or 4Mb pages.
        /// </summary>
        InstructionTlb2M4M = Instruction | Tlb | Page2M | Page4M,

        /// <summary>
        /// A combination describing TLB instruction cache for 4Mb pages.
        /// </summary>
        InstructionTlb4M = Instruction | Tlb | Page4M,

        /// <summary>
        /// A combination describing TLB instruction cache for 1Gb pages.
        /// </summary>
        InstructionTlb1G = Instruction | Tlb | Page1G,

        /// <summary>
        /// A combination describing TLB data cache for 4kb pages.
        /// </summary>
        DataTlb4k = Data | Tlb | Page4k,

        /// <summary>
        /// A combination describing TLB data cache for 4kb and 2Mb pages.
        /// </summary>
        DataTlb4k2M = Data | Tlb | Page4k | Page2M,

        /// <summary>
        /// A combination describing TLB data cache for 4kb and 4Mb pages.
        /// </summary>
        DataTlb4k4M = Data | Tlb | Page4k | Page4M,

        /// <summary>
        /// A combination describing TLB data cache for 2Mb pages.
        /// </summary>
        DataTlb2M = Data | Tlb | Page2M,

        /// <summary>
        /// A combination describing TLB data case for 2Mb or 4Mb pages. The number of entries are often half for
        /// 4M pages.v
        /// </summary>
        DataTlb2M4M = Data | Tlb | Page2M | Page4M,

        /// <summary>
        /// A combination describing TLB data for 4Mb pages.
        /// </summary>
        DataTlb4M = Data | Tlb | Page4M,

        /// <summary>
        /// A combination describing TLB data for 1Gb pages.
        /// </summary>
        DataTlb1G = Data | Tlb | Page1G,

        /// <summary>
        /// A combination describing TLB unified / shared instruction/data cache for 4kb pages.
        /// </summary>
        UnifiedTlb4k = Unified | Tlb | Page4k,

        /// <summary>
        /// A combination describing TLB unified / shared instruction/data cache for 4kb or 2M pages.
        /// </summary>
        UnifiedTlb4k2M = Unified | Tlb | Page4k | Page2M,

        /// <summary>
        /// A combination describing TLB unified / shared instruction/data cache for 1Gb pages.
        /// </summary>
        unifiedTlb1G = Unified | Tlb | Page1G
        #endregion
    }
}
