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
        Module = 3,

        /// <summary>
        /// A Tile.
        /// </summary>
        Tile = 4,

        /// <summary>
        /// A Die on silicon.
        /// </summary>
        Die = 5,

        /// <summary>
        /// A Die group on silicon.
        /// </summary>
        DieGroup = 6,

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
