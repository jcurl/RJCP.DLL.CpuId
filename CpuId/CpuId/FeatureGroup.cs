﻿namespace RJCP.Diagnostics.CpuId
{
    /// <summary>
    /// The group a feature belongs to.
    /// </summary>
    public enum FeatureGroup
    {
        /// <summary>
        /// Unknown feature group.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// A standard feature, described by CPUID.01h.
        /// </summary>
        StandardFeatures,

        /// <summary>
        /// An extended feature, described by CPUID.07h.
        /// </summary>
        StructuredExtendedFeatures,

        /// <summary>
        /// Processor extended states, described by CPUID.0Dh.
        /// </summary>
        ExtendedState,

        /// <summary>
        /// Processor extended function states, described by CPUID.80000001h.
        /// </summary>
        ExtendedFeatures,

        /// <summary>
        /// Processor power management, described by Intel CPUID.06h; AMD CPUID.80000007h.
        /// </summary>
        PowerManagement,

        /// <summary>
        /// Processor extended function states, described by CPUID.80000008h.
        /// </summary>
        ExtendedFeaturesIdentifiers,

        /// <summary>
        /// AMD SVM Feature Identification, described by CPUID.8000000Ah.
        /// </summary>
        SvmFeatures,

        /// <summary>
        /// AMD Performance Optimisation Identifiers, described by CPUID.8000001Ah.
        /// </summary>
        PerformanceOptimizations,

        /// <summary>
        /// AMD Performance Sampling Feature Indicators, described by Intel CPUID.0Ah; AMD CPUID.8000001Bh.
        /// </summary>
        PerformanceSampling,

        /// <summary>
        /// AMD Performance Lightweight Profiling, described by CPUID.8000001Ch.
        /// </summary>
        LightweightProfiling,

        /// <summary>
        /// AMD Encrypted Memory capabilities, described by CPUID.8000001Fh.
        /// </summary>
        EncryptedMemory,

        /// <summary>
        /// AMD PQOS Extended features, described by CPUID.8000_0020h.
        /// </summary>
        PqosExtended,

        /// <summary>
        /// AMD Performance Monitoring and Debug, described by CPUID.8000_0022h.
        /// </summary>
        PerfMonDebug,

        /// <summary>
        /// Intel RDT monitoring, described by CPUID.0Fh, CPUID.10H.
        /// </summary>
        RdtMonitoring,

        /// <summary>
        /// Intel Secure Guard Extensions, described by CPUID.12h.
        /// </summary>
        Sgx,

        /// <summary>
        /// Intel Processor Trace, described by CPUID.14h.
        /// </summary>
        ProcessorTrace,

        /// <summary>
        /// Intel Key Locker, described by CPUID.19h.
        /// </summary>
        KeyLocker,

        /// <summary>
        /// Intel Last Branch Records, described by CPUID.1Ch.
        /// </summary>
        LastBranchRecords
    }
}
