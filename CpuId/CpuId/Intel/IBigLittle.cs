namespace RJCP.Diagnostics.CpuId.Intel
{
    /// <summary>
    /// Interface used for Big/Little architectures on the CPU topology.
    /// </summary>
    public interface IBigLittle
    {
        /// <summary>
        /// Gets a value indicating whether this instance is a performance core.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this instance is a performance core; otherwise, <see langword="false"/>.
        /// </value>
        bool IsPerformanceCore { get; }
    }
}
