namespace RJCP.Diagnostics
{
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
    }
}
