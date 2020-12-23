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
        Smt = 1,

        /// <summary>
        /// A core.
        /// </summary>
        Core = 2,

        /// <summary>
        /// A Module.
        /// </summary>
        Module = 3,

        /// <summary>
        /// TA Tile.
        /// </summary>
        Tile = 4,

        /// <summary>
        /// A Die on silicon.
        /// </summary>
        Die = 5,

        /// <summary>
        /// The logical or physical package.
        /// </summary>
        Package = 256
    }
}
