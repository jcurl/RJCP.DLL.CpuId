namespace RJCP.Diagnostics.CpuId.Intel
{
    /// <summary>
    /// Describes part of the topology of the current CPU core.
    /// </summary>
    public class CpuTopo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CpuTopo"/> class.
        /// </summary>
        /// <param name="id">The identifier for this core, derived from the APIC Id.</param>
        /// <param name="topoType">The topology type for this entry.</param>
        public CpuTopo(long id, CpuTopoType topoType)
        {
            Id = id;
            TopoType = topoType;
        }

        /// <summary>
        /// Gets the identifier, derived from the APIC Id.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; private set; }

        /// <summary>
        /// Gets the topology description for the core that the <see cref="Id"/> represents.
        /// </summary>
        /// <value>The topology type.</value>
        public CpuTopoType TopoType { get; private set; }
    }
}
