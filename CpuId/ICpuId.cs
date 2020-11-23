namespace RJCP.Diagnostics
{
    /// <summary>
    /// Basic information about a CPU.
    /// </summary>
    /// <remarks>
    /// This interface provides the most basic information about a CPU. Use the <see cref="CpuVendor"/> property to
    /// determine the type of the CPU, and typecast to a specific implementation to get more details.
    /// </remarks>
    public interface ICpuId
    {
        /// <summary>
        /// Gets the CPU vendor.
        /// </summary>
        /// <value>The CPU vendor.</value>
        CpuVendor CpuVendor { get; }

        /// <summary>
        /// Gets the vendor identifier, usually a short string.
        /// </summary>
        /// <value>The vendor identifier.</value>
        string VendorId { get; }

        /// <summary>
        /// Gets a more detailed description of the CPU.
        /// </summary>
        /// <value>The detailed description of the CPU.</value>
        string Description { get; }
    }
}
