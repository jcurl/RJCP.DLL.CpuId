namespace RJCP.Diagnostics
{
    using System.Collections.Generic;
    using CpuId;

    /// <summary>
    /// An interface for a CPU factory to get information about a CPU.
    /// </summary>
    public interface ICpuIdFactory
    {
        /// <summary>
        /// Retrieve information about the first CPU.
        /// </summary>
        /// <returns>CPU information.</returns>
        ICpuId Create();

        /// <summary>
        /// Retrieve information about the CPU for a specific core.
        /// </summary>
        /// <param name="core">The core.</param>
        /// <returns>CPU information.</returns>
        ICpuId Create(int core);

        /// <summary>
        /// Retrieves information about all CPUs detected by the Operating System.
        /// </summary>
        /// <returns>An enumerable collection of all CPUs.</returns>
        IEnumerable<ICpuId> CreateAll();
    }
}
