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
        /// <param name="mask">The APIC mask which defines this processor level.</param>
        public CpuTopo(long id, CpuTopoType topoType, long mask)
        {
            Id = id;
            TopoType = topoType;
            Mask = mask;
        }

        /// <summary>
        /// Gets the identifier, derived from the APIC Id.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; }

        /// <summary>
        /// Defines the APIC mask which defines this processor level.
        /// </summary>
        /// <value>The mask.</value>
        /// <remarks>
        /// A value of -1 indicates that the mask is not known. A value of zero indicates that this level is not defined
        /// in the APIC identifier, but a value more than zero doesn't mean that the processor has such a level, only
        /// that there are bits reserved in the APIC identifier, or otherwise known, as the maximum number of possible
        /// cores. The mask also includes lower level cores.
        /// <para>By using the mask, one can build the topology of the CPU compute nodes.</para>
        /// <para>The currently defined hierarchy is:</para>
        /// <list type="bullet">
        /// <item>
        /// <see cref="CpuTopoType.Smt"/>: <c>Key = ApicId &amp; ~Mask;</c>. Logical processors sharing <c>Key</c> are
        /// part of the same <see cref="CpuTopoType.Core"/>.
        /// </item>
        /// <item>
        /// <see cref="CpuTopoType.Core"/>: <c>Key = ApicId &amp; ~Mask;</c>. Logical processors sharing <c>Key</c> are
        /// part of the same <see cref="CpuTopoType.Package"/>.
        /// </item>
        /// <item>
        /// <see cref="CpuTopoType.Node"/>: <c>Key = ApicId &amp; ~Mask;</c>. Logical processors sharing <c>Key</c> are
        /// part of the same <see cref="CpuTopoType.Package"/>. All logical processors within the same package having
        /// the same Id (the same Die, or as AMD calls it, the same NodeId) are on the same Die.
        /// </item>
        /// <item>
        /// <see cref="CpuTopoType.Package"/>: <c>Key = ApicId &amp; Mask;</c>. Contains <see cref="CpuTopoType.Die"/>
        /// and <see cref="CpuTopoType.Core"/>. This is the top level and indicates a physical socket in a
        /// multiprocessor system.
        /// </item>
        /// </list>
        /// </remarks>
        public long Mask { get; }

        /// <summary>
        /// Gets the topology description for the core that the <see cref="Id"/> represents.
        /// </summary>
        /// <value>The topology type.</value>
        public CpuTopoType TopoType { get; }
    }
}
