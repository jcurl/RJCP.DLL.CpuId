namespace RJCP.Diagnostics
{
    /// <summary>
    /// The detected CPU Vendor Type
    /// </summary>
    public enum CpuVendor
    {
        /// <summary>
        /// The CPU Vendor is not recognized by this library.
        /// </summary>
        Unknown,

        /// <summary>
        /// Intel
        /// </summary>
        GenuineIntel,

        /// <summary>
        /// AMD
        /// </summary>
        AuthenticAmd
    }
}
