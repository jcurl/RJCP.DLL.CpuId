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
        /// Retrieve information about the current CPU.
        /// </summary>
        /// <returns>CPU information.</returns>
        ICpuId Create();

        /// <summary>
        /// Retrieves information about all CPUs detected by the Operating System.
        /// </summary>
        /// <returns>An enumerable collection of all CPUs.</returns>
        IEnumerable<ICpuId> CreateAll();
    }
}
