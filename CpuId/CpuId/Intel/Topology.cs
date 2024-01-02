﻿namespace RJCP.Diagnostics.CpuId.Intel
{
    /// <summary>
    /// Describes the core topology.
    /// </summary>
    public class Topology
    {
        /// <summary>
        /// Gets or sets the CPU Initial APIC Id, either basic or extended, depending on the processor.
        /// </summary>
        /// <value>The APIC identifier.</value>
        public long ApicId { get; set; } = -1;

        /// <summary>
        /// Gets information about the core topology.
        /// </summary>
        /// <value>Information about the core topology.</value>
        public CpuTopoList CoreTopology { get; } = new CpuTopoList();

        /// <summary>
        /// Gets information about the cache topology.
        /// </summary>
        /// <value>Information about the cache topology.</value>
        public CacheTopoList CacheTopology { get; } = new CacheTopoList();

        /// <summary>
        /// Gets information if this is a Big / Little core, and extra information associated with it.
        /// </summary>
        /// <value>Information about big / little, or <see langword="null"/> if not available.</value>
        public IBigLittle BigLittle { get; set; }
    }
}
