namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Runtime.Versioning;
    using Native.Win32;

    [SupportedOSPlatform("windows")]
    internal class CpuIdLibRegisters : ICpuRegisters
    {
        private readonly int m_Core;

        /// <summary>
        /// Initializes a new instance of the <see cref="CpuIdLibRegisters"/> class.
        /// </summary>
        /// <param name="core">The core to query for the CPUID registers.</param>
        /// <remarks>Queries the local machine for CPU data.</remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="core"/> is not a valid core number.</exception>
        public CpuIdLibRegisters(int core)
        {
            if (core < 0 || core >= Environment.ProcessorCount)
                throw new ArgumentOutOfRangeException(nameof(core));

            m_Core = core;
        }

        /// <inheritdoc />
        public CpuIdRegister GetCpuId(int function, int subfunction)
        {
            Native.ICpuPin pin = new PinCpuWin32();
            using (pin.Pin(m_Core)) {
                _ = CpuIdLib.cpuid(function, subfunction, out int eax, out int ebx, out int ecx, out int edx);
                return new CpuIdRegister(function, subfunction, new int[] { eax, ebx, ecx, edx });
            }
        }

        /// <inheritdoc />
        public bool IsOnline { get { return true; } }
    }
}
