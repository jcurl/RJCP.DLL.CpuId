namespace RJCP.Diagnostics.CpuId.Intel
{
    /// <summary>
    /// Description of the CPU Topology description.
    /// </summary>
    public enum CpuTopoType
    {
        /// <summary>
        /// The description is invalid.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// Simultaneous Multi-threading, or Hyper-threading, within a Core.
        /// </summary>
        /// <remarks>
        /// This is used for AMD and Intel. A SMT logical processors shares with other SMT logical processors of the same
        /// <see cref="Core"/>.
        /// </remarks>
        Smt = 1,

        /// <summary>
        /// A core.
        /// </summary>
        /// <remarks>
        /// This is used for AMD and Intel. A core (also known as a compute node) is contained within a package. If
        /// there is a processor with 64-cores and SMT, this indicates 128 logical processors, and there will be
        /// 64-cores (not 128), as there are 64 independent compute nodes (for which SMT shares a compute node).
        /// </remarks>
        Core = 2,

        /// <summary>
        /// A Module.
        /// </summary>
        /// <remarks>
        /// This is currently not used by Intel as of i9-10XXX series.
        /// </remarks>
        Module = 3,

        /// <summary>
        /// A Tile.
        /// </summary>
        /// <remarks>
        /// This is currently not used by Intel as of i9-10XXX series.
        /// </remarks>
        Tile = 4,

        /// <summary>
        /// A Die on silicon.
        /// </summary>
        /// <remarks>
        /// This is currently not used by Intel as of i9-10XXX series. This is the Node as given in AMD documentation.
        /// </remarks>
        Die = 5,

        /// <summary>
        /// A Die on Silicon, describing a NodeId on AMD.
        /// </summary>
        Node = 16,

        /// <summary>
        /// The logical or physical package.
        /// </summary>
        Package = 256
    }
}
