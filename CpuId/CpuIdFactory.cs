namespace RJCP.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using CpuId;

    /// <summary>
    /// Factory for getting a class with information about the CPU on the current thread.
    /// </summary>
    public class CpuIdFactory : ICpuIdFactory
    {
        /// <summary>
        /// Retrieve information about the current CPU.
        /// </summary>
        /// <returns>CPU information.</returns>
        /// <exception cref="PlatformNotSupportedException">
        /// OS Platform is not supported.
        /// <para>- or -</para>
        /// Architecture is not supported.
        /// <para>- or -</para>
        /// Cannot load platform specific libraries
        /// <para>- or -</para>
        /// </exception>
        /// <remarks>
        /// Get information about the current CPU. Before doing so, you should set the affinity of the current thread to
        /// be on a specific CPU that you wish to check for.
        /// </remarks>
        public ICpuId Create()
        {
            switch (Environment.OSVersion.Platform) {
            case PlatformID.Win32NT:
                WindowsCpuIdFactory factory = new WindowsCpuIdFactory();
                return factory.Create();
            default:
                throw new PlatformNotSupportedException("OS Platform is not supported");
            }
        }

        /// <summary>
        /// Retrieves information about all CPUs detected by the Operating System.
        /// </summary>
        /// <returns>An enumerable collection of all CPUs.</returns>
        /// <exception cref="PlatformNotSupportedException">
        /// OS Platform is not supported.
        /// <para>- or -</para>
        /// Architecture is not supported.
        /// <para>- or -</para>
        /// Cannot load platform specific libraries
        /// <para>- or -</para>
        /// </exception>
        /// <remarks>
        /// Get information about the current CPU. Before doing so, you should set the affinity of the current thread to
        /// be on a specific CPU that you wish to check for.
        /// </remarks>
        public IEnumerable<ICpuId> CreateAll()
        {
            switch (Environment.OSVersion.Platform) {
            case PlatformID.Win32NT:
                WindowsCpuIdFactory factory = new WindowsCpuIdFactory();
                return factory.CreateAll();
            default:
                throw new PlatformNotSupportedException("OS Platform is not supported");
            }
        }
    }
}
